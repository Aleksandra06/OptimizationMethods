namespace TransportTask
{
    internal class Program
    {
        public class GrafModel
        {
            public int C { get; set; }//склад
            //public int CW { get; set; }//склад вес
            public int M { get; set; }//магазин
            //public int MW { get; set; }//магазин вес
            public int W { get; set; }//вес
            public int Number { get; set; }//кол-вод
            //public int U { get; set; }//кол-вод
            //public int V { get; set; }//кол-вод
        }
        static void Main(string[] args)
        {
            var data = File.ReadAllLines("graf.txt");
            var graf = new List<GrafModel>();
            foreach (var row in data)
            {
                var tmp = new List<int>();
                var col = row.Split("\t");
                foreach (var item in col)
                {
                    tmp.Add(int.Parse(item));
                }
                graf.Add(new GrafModel()
                {
                    C = tmp[0],
                    M = tmp[1],
                    W = tmp[2],
                    //CW = -1,
                    //MW = -1,
                    Number = -1
                });
            }
            data = File.ReadAllLines("magazin.txt");
            var M = new List<int>();
            foreach (var row in data)
            {
                M.Add(int.Parse(row));
                var index = graf.FindIndex(x => x.M == M.Count - 1);
                //graf[index].MW = M.Last();
            }
            data = File.ReadAllLines("sklad.txt");
            var C = new List<int>();
            foreach (var row in data)
            {
                C.Add(int.Parse(row));
                var index = graf.FindIndex(x => x.C == C.Count - 1);
                //graf[index].CW = C.Last();
            }
            Run(C, M, graf);
        }

        private static void Run(List<int> C, List<int> M, List<GrafModel> graf)
        {
            Ravn(C, M, graf);
            CalcGrafOne(graf, C.ToList(), M.ToList());
            var shag = 1;
            while (true)
            {
                Console.WriteLine($"\nШаг {shag}:\n");
                var zanyat = graf.Where(x => x.Number > 0).Count();
                Console.WriteLine($"Занятых клеток: {zanyat}; n + m - 1 = {C.Count + M.Count - 1}\n");
                List<int> U = InitIntList(C.Count());
                List<int> V = InitIntList(M.Count());
                CalcUV(graf, U, V);
                PrintGraf(C, M, U, V, graf);
                var indexMax = FingIndexMax(graf, U, V);
                if (indexMax == -1)
                {
                    break;
                }
                //var table = CreateTable(C, M);
                //PrintTable(table, C, M);
            }
        }

        private static int FingIndexMax(List<GrafModel> graf, List<int> u, List<int> v)//u->c, v-> m
        {
            var index = -1;
            var max = -1;
            for (var i = 0; i < graf.Count(); i++)
            {
                var g = graf[i];
                if (g.Number > 0)
                {
                    var tmp = u[g.C] + v[g.M] - g.W;
                    if (tmp > 0 && max < tmp)
                    {
                        index = i;
                        max = tmp;
                    }
                }
            }

            return index;
        }

        private static void CalcUV(List<GrafModel> graf, List<int> U, List<int> V)//u->c, v-> m
        {
            Queue<int> q = new Queue<int>();
            for (int i = 0; i < graf.Count; i++)
            {
                q.Enqueue(i);
            }
            while (q.Count > 0)
            {
                var id = q.Dequeue();
                var g = graf[id];
                if (g.Number <= 0)
                {
                    continue;
                }
                if (g.M == 0)
                {
                    V[g.M] = 0;
                    U[g.C] = g.W;
                }
                else
                {
                    if (V[g.M] == int.MinValue && U[g.C] == int.MinValue)
                    {
                        q.Enqueue(id);
                    }
                    else if (V[g.M] == int.MinValue)
                    {
                        V[g.M] = g.W - U[g.C];
                    }
                    else if (U[g.C] == int.MinValue)
                    {
                        U[g.C] = g.W - V[g.M];
                    }
                }
            }
        }

        private static void PrintGraf(List<int> c, List<int> m, List<int> u, List<int> v, List<GrafModel> graf)//u->c, v-> m
        {
            Console.Write("\t");
            for (int j = 0; j < m.Count; j++)
            {
                Console.Write(m[j] + "\t");
            }
            Console.Write("U\t");
            var group = graf.GroupBy(x => x.C).ToList();
            for (int i = 0; i < c.Count(); i++)
            {
                Console.WriteLine("\n");
                Console.Write(Print(c[i]) + "\t");
                var g = group[i].ToList();
                for (int j = 0; j < m.Count(); j++)
                {
                    var index = g.FindIndex(x => x.M == j);
                    if (index >= 0 && g[index].Number >= 0)
                    {
                        Console.Write($"{g[index].Number}({g[index].W})\t");
                    }
                    else
                    {
                        Console.Write("-\t");
                    }
                }
                Console.Write(u[i] + "\t");
            }
            Console.Write("\nV\t");
            for (int j = 0; j < v.Count; j++)
            {
                Console.Write(Print(v[j]) + "\t");
            }
            Console.WriteLine("\n");
        }

        static string Print(int number)
        {
            if (number == int.MinValue)
            {
                return "-";
            }
            else { return number.ToString(); }
        }

        private static void CalcGrafOne(List<GrafModel> graf, List<int> C, List<int> M)
        {
            foreach (var g in graf)
            {
                if (C[g.C] != 0 && M[g.M] != 0)
                {
                    if (C[g.C] > M[g.M])
                    {
                        g.Number = M[g.M];
                        C[g.C] = C[g.C] - M[g.M];
                        M[g.M] = 0;
                    }
                    else
                    {
                        g.Number = C[g.C];
                        M[g.M] = M[g.M] - C[g.C];
                        C[g.C] = 0;
                    }
                }
            }
        }

        private static void PrintTable(List<List<int>> table, List<int> c, List<int> m)
        {
            Console.Write("\t");
            for (int j = 0; j < m.Count; j++)
            {
                Console.Write(m[j] + "\t");
            }
            for (int i = 0; i < c.Count; i++)
            {
                Console.Write('\n');
                Console.Write(c[i] + "\t");
                for (int j = 0; j < m.Count; j++)
                {
                    Console.Write(table[i][j] + "\t");
                }
            }
            Console.Write("\n");
        }
        private static List<List<int>> CreateTable(List<int> c, List<int> m)
        {
            var C = c.ToList();
            var M = m.ToList();
            List<List<int>> table = new List<List<int>>();
            for (int i = 0; i < C.Count; i++)
            {
                table.Add(new List<int>());
                for (int j = 0; j < M.Count; j++)
                {
                    if (M[j] <= C[i])
                    {
                        table.Last().Add(M[j]);
                        M[j] = 0;
                        C[i] = C[i] - M[j];
                    }
                    else
                    {
                        table.Last().Add(C[i]);
                        M[j] = M[j] - C[i];
                        C[i] = 0;
                    }
                }
            }
            return table;
        }

        private static void Ravn(List<int> c, List<int> m, List<GrafModel> graf)
        {
            if (c.Sum() > m.Sum())
            {
                m.Add(c.Sum() - m.Sum());
                for (int i = 0; i < c.Count; i++)
                {
                    graf.Add(new GrafModel() { C = i, M = m.Count() - 1, W = 0, Number = -1 });
                }
            }
            else if (c.Sum() < m.Sum())
            {
                c.Add(m.Sum() - c.Sum());
                for (int i = 0; i < m.Count; i++)
                {
                    graf.Add(new GrafModel() { C = c.Count() - 1, M = i, W = 0, Number = -1 });
                }
            }
        }

        static List<int> InitIntList(int count)
        {
            var list = new List<int>();
            for (int i = 0; i < count; i++)
            {
                list.Add(int.MinValue);
            }
            return list;
        }
        static List<bool> InitBoolList(int count)
        {
            var list = new List<bool>();
            for (int i = 0; i < count; i++)
            {
                list.Add(false);
            }
            return list;
        }
        static List<List<int>> InitMas(int n, int m)
        {
            var mass = new List<List<int>>();
            for (int i = 0; i < n; i++)
            {
                var tmp = new List<int>();
                for (int j = 0; j < m; j++)
                {
                    tmp.Add(0);
                }
                mass.Add(tmp);
            }

            return mass;
        }
    }
}