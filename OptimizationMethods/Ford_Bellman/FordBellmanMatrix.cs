using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ford_Bellman
{
    public class FordBellmanMatrix
    {
        readonly int inf = -1;
        public void Run()
        {
            var path = "input_lab_finding_shortest_distance_Ford-Bellman.txt";
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
            if (!IsNormGraf(graf))
            {
                Console.WriteLine("Данные графа не верны");
                return;
            }
            var startNode = 0;
            if (startNode >= 0 && startNode < graf.Count)
            {
                FordBellman(graf, startNode);
            }
        }
        private void FordBellman(List<List<int>> graf, int start)
        {
            var n = graf.Count();
            var D = InitMas(n + 1, n);
            var puti = InitMas(n, n);
            int min;

            for (int i = 0; i < n; i++)
            {
                if (i == start)
                    D[0][i] = 0;
                else
                    D[0][i] = inf;
            }

            WriteD(D, n, 0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    puti[i][j] = -1;
                puti[i][0] = start;
            }
            var d = new List<int>();
            d.Add(0);
            for (int j = 1; j < n; j++)
            {
                d.Add(inf);
            }

            for (int i = 1; i < n; i++)
            {
                var ism = 0;
                var dprio = d.ToList();
                for (int j = 0; j < n; j++)
                {
                    min = dprio[j];
                    for (int k = 0; k < n; k++)
                    {
                        if ((min > dprio[k] + graf[k][j] && dprio[k] != inf && graf[k][j] != inf)
                            || (min == inf && dprio[k] != inf && graf[k][j] != inf))
                        {
                            min = dprio[k] + graf[k][j];
                            puti_(puti, j, k, n);
                        }
                    }
                    if (d[j] != min)
                    {
                        ism++;
                    }
                    d[j] = min;
                }
                Writed(d, n, i, ism);
                int flag = 0;
                for (int j = 0; j < n; j++)
                {
                    if (dprio[j] == d[j]) flag++;
                }
                if (flag == n) break;
            }

            WritePuti(puti, n);
        }
        void WritePuti(List<List<int>> puti, int n)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write($"\nv {j + 1} - ");
                for (int i = 0; i < n; i++)
                {
                    if (puti[j][i] > -1)
                    {
                        Console.Write($"{puti[j][i] + 1} ");
                    }
                }
            }
        }
        void Writed(List<int> d, int n, int row, int ism)
        {
            Console.Write($"\nD({row})=(");
            for (int l = 0; l < n; l++)
            {
                if (d[l] == inf)
                    Console.Write("\t-");
                else
                    Console.Write($"\t{d[l]}");
            }
            Console.Write(") " + ism);
        }
        void WriteD(List<List<int>> D, int n, int row)
        {

            Console.Write($"\nD({row})=(");
            for (int l = 0; l < n; l++)
            {
                if (D[row][l] == inf)
                    Console.Write("\t-");
                else
                    Console.Write($"\t{D[row][l]}");
            }
            Console.Write(")");
        }
        List<List<int>> InitMas(int n, int m)
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
        void puti_(List<List<int>> puti, int j, int k, int n)
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
        bool IsNormGraf(List<List<int>> graf)
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
