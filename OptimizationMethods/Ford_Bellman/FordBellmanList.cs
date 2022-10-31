using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ford_Bellman
{
    public class FordBellmanList
    {
        readonly int inf = int.MaxValue;
        public class GrafModel
        {
            public int A { get; set; }//из
            public int B { get; set; }//в
            public int W { get; set; }//вес
        }
        public void Run()
        {
            var path = "input_lab_finding_shortest_distance_Ford-BellmanList.txt";
            var data = File.ReadAllLines(path);
            var graf = GenerationGraf();
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
            //        A = tmp[0],
            //        B = tmp[1],
            //        W = tmp[2]
            //    });
            //}
            //LookList(graf);
            var startNode = 0;
            FordBellman(graf, startNode);
        }

        private void FordBellman(List<GrafModel> graf, int startNode, bool idFindPuti = false)
        {
            var countA = graf.Select(x => x.A).Max();
            var countB = graf.Select(x => x.B).Max();
            var count = countA > countB ? countA : countB;
            var d = InitD(count, startNode);
            //Writed(d, count, 0);
            var dprio = d.ToList();
            //for (int i = 0; i < count; i++)
            //{
            //    for (int j = 0; j < count; j++)
            //        puti[i][j] = -1;
            //    puti[i][0] = start;
            //}
            int min;
            int i;
            for (i = 1; i < count; i++)//d
            {
                dprio = d.ToList();
                for (int u = 0; u < count; u++)//dprio. u
                {
                    min = dprio[u];
                    for (int v = 0; v < count; v++)//find min
                    {
                        var puti = graf.Where(x => (x.A == v + 1 && x.B == u + 1) || (x.B == v + 1 && x.A == u + 1)).ToList();
                        if (puti.Count == 0 || dprio[v] == inf)
                        {
                            continue;
                        }
                        var put = puti.Select(x => x.W).Min();
                        if (min == inf)
                        {
                            if (dprio[v] != inf)
                            {
                                min = dprio[v] + put;
                            }
                            continue;
                        }
                        if (min > dprio[v] + put)
                        {
                            min = dprio[v] + put;
                            //puti_(puti, j, k, n);
                        }
                    }
                    d[u] = min;
                }
                //Writed(d, count, i);
                int flag = 0;
                for (int j = 0; j < count; j++)
                {
                    if (dprio[j] == d[j]) flag++;
                }
                if (flag == count) break;
            }

            Writed(d, count, i);
        }
        void Writed(List<int> d, int n, int row)
        {
            Console.Write($"\nD({row})=(");
            for (int l = 0; l < n; l++)
            {
                if (d[l] == inf)
                    Console.Write("\t-");
                else
                    Console.Write($"\t{d[l]}");
            }
            Console.Write(")");
        }

        private List<int> InitD(int count, int start)
        {
            var d = new List<int>();
            for (int i = 0; i < count; i++)
            {
                d.Add(inf);
            }
            d[start] = 0;
            return d;
        }

        private void LookList(List<GrafModel> graf)
        {
            foreach (var item in graf)
            {
                Console.Write($"\n{item.A}<->{item.B}={item.W}");
            }
        }

        private List<GrafModel> GenerationGraf()
        {
            var graf = new List<GrafModel>();
            var rows = 1000;
            var cols = 1000;
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var item = (row * cols) + col;
                    if (col + 1 < cols)
                    {
                        graf.Add(new GrafModel() { A = item, B = item + 1, W = 1 });
                    }
                    if (row + 1 < rows)
                    {
                        graf.Add(new GrafModel() { A = item, B = item + cols, W = 1 });
                    }
                }
            }
            return graf;
        }
    }
}

//1   2   7
//1   4   20
//1   3   3
//1   6   8
//2   4   6
//3   5   4
//3   6   4
//4   5   5

