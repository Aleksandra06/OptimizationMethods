using System.Collections.Generic;
using System.Globalization;
using static System.Reflection.Metadata.BlobBuilder;

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
            var data = File.ReadAllLines("task.txt");
            var graf = new List<GrafModel>();
            var C = new List<int>();
            var M = new List<int>();
            for (int r = 0; r < data.Length - 1; r++)
            {
                var col = data[r].Split("\t");
                for (int c = 0; c < col.Length - 1; c++)
                {
                    string? item = col[c];
                    graf.Add(new GrafModel()
                    {
                        C = r,
                        M = c,
                        W = int.Parse(item),
                        Number = -1
                    });
                }
                var intem = col[col.Length - 1];
                C.Add(int.Parse(intem));
            }
            var colM = data[data.Length - 1].Split("\t");
            for (int c = 0; c < colM.Length; c++)
            {
                string? item = colM[c];
                M.Add(int.Parse(item));
            }
            PrintGraf(C, M, null, null, graf);
            //var data = File.ReadAllLines("graf.txt");
            //var graf = new List<GrafModel>();
            //foreach (var row in data)
            //{
            //    var tmp = new List<int>();
            //    var col = row.Split("\t");
            //    foreach (var item in col)
            //    {
            //        tmp.Add(int.Parse(item));
            //    }
            //    graf.Add(new GrafModel()
            //    {
            //        C = tmp[0],
            //        M = tmp[1],
            //        W = tmp[2],
            //        //CW = -1,
            //        //MW = -1,
            //        Number = -1
            //    });
            //}
            //data = File.ReadAllLines("magazin.txt");
            //var M = new List<int>();
            //foreach (var row in data)
            //{
            //    M.Add(int.Parse(row));
            //    var index = graf.FindIndex(x => x.M == M.Count - 1);
            //    //graf[index].MW = M.Last();
            //}
            //data = File.ReadAllLines("sklad.txt");
            //var C = new List<int>();
            //foreach (var row in data)
            //{
            //    C.Add(int.Parse(row));
            //    var index = graf.FindIndex(x => x.C == C.Count - 1);
            //    //graf[index].CW = C.Last();
            //}
            Run(C, M, graf);
        }

        private static void Run(List<int> C, List<int> M, List<GrafModel> graf)
        {
            graf = Ravn(C, M, graf);
            CalcGrafOne(graf, C.ToList(), M.ToList());
            var Fx = new List<int>();
            var shag = 1;
            while (true)
            {
                if (shag == 4)
                {

                }
                Console.WriteLine($"\nШаг {shag}:\n");
                var zanyat = graf.Where(x => x.Number >= 0).Count();
                Console.WriteLine($"Занятых клеток: {zanyat}; n + m - 1 = {C.Count + M.Count - 1}\n");
                while (C.Count + M.Count - 1 > zanyat)
                {
                    PrintGraf(C, M, null, null, graf);
                    FixGraf(graf, M.Count, C.Count);
                    zanyat = graf.Where(x => x.Number >= 0).Count();
                    Console.WriteLine($"Занятых клеток: {zanyat}; n + m - 1 = {C.Count + M.Count - 1}\n");
                    PrintGraf(C, M, null, null, graf);
                }
                var fx = CalcFx(graf);
                Fx.Add(fx);
                Console.WriteLine($"F(x) = {fx}\n");
                List<int> U = InitIntList(C.Count());
                List<int> V = InitIntList(M.Count());
                var vurozden = !CalcUV(graf, U, V);
                while (vurozden)
                {
                    Console.WriteLine($"Невозможно расчитать все U, V!\n");
                    //PrintGraf(C, M, U, V, graf);
                    FixVurozdenGraf(graf, U, V);
                    vurozden = !CalcUV(graf, U, V);
                }
                PrintGraf(C, M, U, V, graf);
                var indexMax = FingIndexMax(graf, U, V);
                if (indexMax == -1)
                {
                    Console.WriteLine($"Опорный план является оптимальным, так все оценки свободных клеток удовлетворяют условию ui + vj <= cij.\n");
                    break;
                }
               // Console.WriteLine($"ui + vj - cij = {U[graf[indexMax].C] + V[graf[indexMax].M] - graf[indexMax].Number}\n");
                ChangeGrafByIndex(graf, indexMax);
                //PrintGraf(C, M, U, V, graf);
                //var table = CreateTable(C, M);
                //PrintTable(table, C, M);

                shag++;
            }
        }

        private static void FixVurozdenGraf(List<GrafModel> graf, List<int> U, List<int> V)//u->c, v-> m
        {
            var listId = new List<int>();
            for (int i = 0; i < U.Count; i++)
            {
                int u = U[i];
                if (u == int.MinValue)
                {
                    var list = graf.Where(x => x.C == i && x.Number == -1).ToList();
                    foreach (var item in list)
                    {
                        var id = graf.FindIndex(x => x.C == item.C && x.M == item.M);
                        listId.Add(id);
                    }
                }
            }
            for (int i = 0; i < V.Count; i++)
            {
                int v = V[i];
                if (v == int.MinValue)
                {
                    var list = graf.Where(x => x.M == i && x.Number == -1).ToList();
                    foreach (var item in list)
                    {
                        var id = graf.FindIndex(x => x.C == item.C && x.M == item.M);
                        listId.Add(id);
                    }
                }
            }
            var min = int.MaxValue;
            var idMin = -1;
            foreach (var id in listId)
            {
                if (graf[id].W < min)
                {
                    min = graf[id].W;
                    idMin = id;
                }
            }

            var idZero = graf.FindIndex(x => x.Number == 0);
            if (idZero >= 0)
            {
                graf[idZero].Number = -1;
                graf[idMin].Number = 0;
            }
        }

        private static void FixGraf(List<GrafModel> graf, int M, int cCount)
        {
            for (int i = 0; i < cCount; i++)
            {
                var rows = graf.Where(x => x.C == i).ToList();
                var index = rows.FindIndex(x => x.Number == -1);
                if (index >= 0)
                {
                    var gIndex = graf.FindIndex(x => x.C == rows[index].C && x.M == rows[index].M);
                    graf[gIndex].Number = 0;
                    return;
                }
            }
        }

        private static int CalcFx(List<GrafModel> graf)
        {
            var sum = 0;
            foreach (var g in graf)
            {
                if (g.Number > 0)
                {
                    sum += g.Number * g.W;
                }
            }
            return sum;
        }

        private static void ChangeGrafByIndex(List<GrafModel> graf, int indexMax)
        {
            var put = FindPut(graf, indexMax, indexMax, null, true).Distinct().ToList();
            var minNumber = FindMinNumber(graf, put);
            Console.WriteLine($"min = {minNumber}, C = {graf[indexMax].C + 1}, M = {graf[indexMax].M + 1}\n");
            Console.Write($"Путь:");
            foreach (var p in put)
            {
                Console.Write($"{graf[p].C + 1}.{graf[p].M + 1},");
            }
            Console.Write($"\n");
            var minus = -1;
            for (int i = 0; i < put.Count - 1; i++)
            {
                int p = put[i];
                graf[p].Number = graf[p].Number + minus * minNumber;
                minus = minus * -1;
                if (graf[p].Number == 0)
                {
                    graf[p].Number = -1;
                }
            }
            graf[indexMax].Number = minNumber;
        }

        private static int FindMinNumber(List<GrafModel> graf, List<int> put)
        {
            var minus = 1;
            var min = graf[put[0]].Number;
            for (int i = 1; i < put.Count - 1; i++)
            {
                if (min > graf[put[i]].Number && minus < 0)
                {
                    min = graf[put[i]].Number;
                }
                minus = minus * -1;
            }
            return min;
        }

        private static List<int> FindPut(List<GrafModel> graf, int startId, int endId, bool? isDown, bool isStart = false)
        {
            if (endId == startId && !isStart)
            {
                return new List<int>() { endId };
            }
            var nextIdList = FindNextId(graf, startId, isDown, endId);
            foreach (var id in nextIdList)
            {
                isDown = isStart ? graf[startId].M == graf[id].M : isDown.Value;
                var put = FindPut(graf, id, endId, !isDown);
                if (put == null)
                {
                    continue;
                }
                put.Insert(0, id);
                return put;
            }
            return null;
        }

        private static List<int> FindNextId(List<GrafModel> graf, int indexMax, bool? isDown, int endId)
        {
            if (isDown == true || isDown == null)
            {
                var list = graf.Where(x => x.M == graf[indexMax].M && x.C != graf[indexMax].C).ToList();
                if (list.Any(x => x.M == graf[endId].M && x.C == graf[endId].C))
                {
                    return new List<int>() { endId };
                }
                else
                {
                    list = list.Where(x => x.Number >= 0).ToList();
                }
                var nIndex = new List<int>();
                foreach (var l in list)
                {
                    var n = graf.FindIndex(x => x.C == l.C && x.M == l.M);
                    nIndex.Add(n);
                }
                return nIndex;
            }
            if (isDown == false || isDown == null)
            {
                var list = graf.Where(x => x.C == graf[indexMax].C && x.M != graf[indexMax].M).ToList();
                if (list.Any(x => x.M == graf[endId].M && x.C == graf[endId].C))
                {
                    return new List<int>() { endId };
                }
                else
                {
                    list = list.Where(x => x.Number >= 0).ToList();
                }
                var nIndex = new List<int>();
                foreach (var l in list)
                {
                    var n = graf.FindIndex(x => x.C == l.C && x.M == l.M);
                    nIndex.Add(n);
                }
                return nIndex;
            }
            return new List<int>();
        }
        private static int FingIndexMax(List<GrafModel> graf, List<int> u, List<int> v)//u->c, v-> m
        {
            var index = -1;
            var max = -1;
            for (var i = 0; i < graf.Count(); i++)
            {
                var g = graf[i];
                if (g.Number < 0)
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

        private static bool CalcUV(List<GrafModel> graf, List<int> U, List<int> V)//u->c, v-> m
        {
            Queue<int> q = new Queue<int>();
            for (int i = 0; i < graf.Count; i++)
            {
                q.Enqueue(i);
            }
            var oneId = -1;
            var count = 0;
            while (q.Count > 0)
            {
                var id = q.Dequeue();
                var g = graf[id];
                if (g.Number < 0)
                {
                    continue;
                }
                if (g.C == 0)
                {
                    V[g.M] = g.W;
                    U[g.C] = 0;
                    oneId = -1;
                }
                else
                {
                    if (V[g.M] == int.MinValue && U[g.C] == int.MinValue)
                    {
                        q.Enqueue(id);
                        if (q.Count == 1)
                        {
                            if (q.Peek() == oneId)
                            {
                                count++;
                                if (count > 3)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                oneId = q.Peek();
                            }
                        }
                    }
                    else if (V[g.M] == int.MinValue)
                    {
                        V[g.M] = g.W - U[g.C];
                        oneId = -1;
                    }
                    else if (U[g.C] == int.MinValue)
                    {
                        U[g.C] = g.W - V[g.M];
                        oneId = -1;
                    }
                }
            }
            return true;
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
                        Console.Write($"{g[index].W}({g[index].Number})\t");
                    }
                    else
                    {
                        Console.Write($"{g[index].W}\t");
                    }
                }
                if (u != null)
                {
                    Console.Write(u[i] + "\t");
                }
            }
            Console.Write("\n\nV\t");
            if (v != null)
            {
                for (int j = 0; j < v.Count; j++)
                {
                    Console.Write(Print(v[j]) + "\t");
                }
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
            // var mark = InitMas(C.Count, M.Count);// 0 - ничего, 1 - марк, 2 - готово
            foreach (var g in graf)
            {
                if (C[g.C] != 0 && M[g.M] != 0)
                {
                    if (C[g.C] > M[g.M])
                    {
                        g.Number = M[g.M];
                        C[g.C] = C[g.C] - M[g.M];
                        M[g.M] = 0;
                        //mark[g.C][g.M] = 2;
                    }
                    else
                    {
                        g.Number = C[g.C];
                        M[g.M] = M[g.M] - C[g.C];
                        C[g.C] = 0;
                        //mark[g.C][g.M] = 2;
                    }
                    //if (M[g.M] == 0 && g.M - 1 > 1 && mark[g.C][g.M - 1] == 2)
                    //{
                    //    for (int i = 0; i < mark[g.C].Count; i++)
                    //    {
                    //        int m = mark[g.C][i];
                    //    }
                    //}
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

        private static List<GrafModel> Ravn(List<int> c, List<int> m, List<GrafModel> graf)
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
            graf = graf.OrderBy(x => x.M).OrderBy(x => x.C).ToList();
            return graf;
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

//1
//3   5   4   6   50
//2   4   1   5   75
//8   3   2   4   100
//2   5   6   1   120
//40  80  110 70
//2
//2   4   6   3   1   7
//3   5   2   7   3   8
//2   1   3   1   5   15
//6   5   7   4   5
//3
//4   5   8   1   15
//2   6   5   4   10
//1   3   2   5   1
//1   3   3   1   5
//7   8   3   5
//4
//4   5   2   3   8
//2   5   2   4   5
//3   1   7   3   6
//7   1   6   4
//5
//4   4   5   2   7
//5   6   1   4   6
//1   1   3   5   5
//4   8   1   5
//5
//4   4   5   10
//5   6   1   10
//1   1   3   10
//10  10  10
//6
//3   20  8   13  4   100 80
//4   4   18  14  3   0   60
//10  4   18  8   6   0   30
//7   19  17  10  1   100 60
//10  30  40  50  70  30
//7
//4   4   5   7   6   10
//5   6   1   6   5   10
//1   1   3   4   3   10
//4   3   7   5   1   10
//5   6   9   4   2   10
//10  10  10  10  10
