using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Dijkstra.Program;

namespace Dijkstra
{
    class Program
    {
        public class Table
        {
            public List<int> D { get; set; }
            public int DW { get; set; }
            public int W { get; set; }
            public List<int> put { get; set; }
        }

        static readonly int inf = 99999;
        static void Main(string[] args)
        {
            int n, start;
            var path = "input_lab_finding_shortest_distance_Dijkstra.txt";
            var data = File.ReadAllLines(path);
            var graf = new List<List<int>>();
            foreach (var row in data)
            {
                Console.WriteLine($"{row}");
                var tmp = new List<int>();
                var col = row.Split("\t");
                foreach (var item in col)
                {
                    tmp.Add(int.Parse(item));
                }
                graf.Add(tmp);
            }
            n = graf.Count;
            if (!IsNormGraf(graf))
            {
                Console.WriteLine("Данные графа не верны");
                return;
            }
            PrintGraf(graf);
            start = 0;
            var table = Dijkstra(graf, n, start);
            //PrintTable(table, n);
            //cout << "Востановление пути:" << endl;
            //int* min;
            //for (int i = 0; i < n; i++)
            //{
            //    min = RestoringPath(n, i);
            //    int j = 0;
            //    while (min[j] != -1 && j < n)
            //    {
            //        cout << min[j] + 1 << " ";
            //        j++;
            //    }
            //    cout << "=" << min[n] << endl;
            //}
        }

        static void puti_(List<List<int>> puti, int j, int k, int n)
        {
            for (int s = 1; s < n; s++)
            {
                if (puti[k][s] != (-1))
                    puti[j][s] = puti[k][s];
                else
                {
                    puti[j][s] = j;
                    break;
                }
            }
        }
        private static void PrintTable(List<Table> table, int n)
        {
            Console.WriteLine("+-------------------------------------------------------------------------------");
            Console.Write("| W\t| D(W)\t|");
            for (int i = 0; i < n; i++)
            {
                Console.Write(" " + (i + 1) + "\t|");
            }
            Console.Write(" S\n");
            Console.WriteLine("+-------------------------------------------------------------------------------");
            for (int i = 0; i < n; i++)
            {
                Console.Write("| " + print(table[i].W + 1) + "\t| " + print(table[i].DW) + "\t|");
                if (i == 0)
                {
                    for (int j = 0; j < n; j++)
                    {
                        Console.Write(print(table[i].D[j]) + "\t|");
                    }
                }
                else
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (table[i].D[j] == 0) Console.Write(" -\t|");
                        else Console.Write(print(table[i].D[j]) + "\t|");
                    }
                }
                var p = table[i].put;
                for (int k = 0; k < i + 1; k++)
                {
                    Console.Write((p[k] + 1) + ", ");
                }
                Console.Write("\n");
                Console.WriteLine("+-------------------------------------------------------------------------------");
            };
        }
        static string print(int n)
        {
            if (n >= inf) return "-";
            else
            {
                return n.ToString();
            }
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
        private static List<Table> Dijkstra(List<List<int>> matrix, int n, int start)
        {
            var puti = InitMas(n, n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    puti[i][j] = -1;
                puti[i][0] = start;
            }
            var table = new List<Table>();
            for (int i = 0; i < n; i++)
            {
                table.Add(new Table());
                table[i].D = InitIntList(n);
                table[i].DW = 0;
                table[i].W = 0;
            }
            //0
            for (int i = 0; i < n; i++)
            {
                table[0].D[i] = matrix[start][i];
            }
            table[0].put = new List<int>();
            table[0].put.Add(start);
            table[0].DW = inf;
            table[0].W = inf;

            int W, DW;
            var bul = InitBoolList(n);
            bul[start] = true;
            for (int i = 1; i < n; i++)
            {
                var minVal = table[i - 1].D.Where(x => x > 0).Min();
                W = table[i - 1].D.FindIndex(x => x == minVal);
                bul[W] = true;
                var x = puti[W].FindIndex(x => x == -1);
                puti[W][x] = W;
                DW = table[i - 1].D[W];
                table[i].W = W;
                table[i].DW = DW;
                table[i].put = table[i - 1].put.ToList();
                table[i].put.Add(W);
                for (int j = 0; j < n; j++)
                {
                    if (!bul[j])
                    {
                        if (table[i - 1].D[j] > DW + matrix[W][j])
                        {
                            table[i].D[j] = DW + matrix[W][j];
                            puti_(puti, j, W, n);
                        }
                        else
                        {
                            table[i].D[j] = table[i - 1].D[j];
                            //puti_(puti, j, j, n);
                        }
                    }
                }
            }
            for (int i = 0; i < puti.Count; i++)
            {
                puti[i] = puti[i].Distinct().Where(x => x != -1).ToList();
            }
            PrintTable(table, n);
            WritePuti(puti, n);
            Console.WriteLine("");
            return table;
        }
        static void WritePuti(List<List<int>> puti, int n)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write($"\nv {j + 1} - ");
                foreach (var ver in puti[j])
                {
                    if (ver > -1)
                    {
                        Console.Write($"{ver + 1} ");
                    }
                }
            }
        }

        static List<int> InitIntList(int count)
        {
            var list = new List<int>();
            for (int i = 0; i < count; i++)
            {
                list.Add(0);
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
        static void PrintGraf(List<List<int>> C)
        {
            Console.Write("\n");
            var n = C.Count;
            //for (int i = 0; i < n; i++)
            //{
            //    Console.Write("\t" + i + 1);
            //}
            Console.Write("\n");
            for (int i = 0; i < n; i++)
            {
                //Console.Write("\t" + i + 1);
                for (int j = 0; j < n; j++)
                {
                    if (C[i][j] == inf)
                        Console.Write("\t-");
                    else Console.Write("\t" + C[i][j]);
                }
                Console.Write("\n");
            }
        }
        static bool IsNormGraf(List<List<int>> graf)
        {
            var numer = graf.Count;
            foreach (var row in graf)
            {
                if (row.Count != numer)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

//0	7	6	4	2	4	99999
//1   0   2   6   99999   1   4
//2   4   0   6   8   6   2
//1   99999   6   0   2   3   2
//4   2   3   7   0   1   6
//4   7   4   99999   2   0   1
//5   99999   3   1   9   1   0