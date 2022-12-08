using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MaximumFlow
{
    class Program
    {
        readonly int inf = -1;
        public class GrafModel
        {
            public int A { get; set; }//из
            public int B { get; set; }//в
            public int W { get; set; }//вес
        }
        static void Main(string[] args)
        {
            var path = "graf.txt";
            var data = File.ReadAllLines(path);
            // var graf = GenerationGraf();
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
                    A = tmp[0] - 1,
                    B = tmp[1] - 1,
                    W = tmp[2]
                });
            }
            LookList(graf);
            var startNode = 0;
            var endNode = 5;
            var potok = MaxFlow(graf, startNode, endNode);
            Console.WriteLine("\n\nМаксимальный поток: " + potok);
        }

        private static int MaxFlow(List<GrafModel> graf, int startNode, int endNode)
        {
            var maxPotok = 0;
            var iter = 1;
            while (true)
            {
                ////BFS
                var put = SearchPutBFS(graf, startNode, endNode);
                if (put == null || put?.Count == 0)
                {
                    return maxPotok;
                }
                ///DFS
                //var a = graf.Select(x => x.A).Max();
                //var b = graf.Select(x => x.B).Max();
                //var n = a > b ? a : b;
                //var f = SearchPutDFS(graf, startNode, endNode, InitIntList(n + 1));
                //if (!f.Item1)
                //{
                //    return maxPotok;
                //}
                Console.WriteLine("\n\n******" + iter);
                iter++;
                //var put = f.Item2;
                //////
                //Console.Write("\nPut:");
                LookPut(put);
                var listIndex = new List<int>();
                var w = new List<int>();
                for (int i = 0; i < put.Count - 1; i++)
                {
                    var index = graf.FindIndex(x => x.A == put[i] && x.B == put[i + 1]);
                    listIndex.Add(index);
                    w.Add(graf[index].W);
                }
                var minW = w.Min();
                maxPotok += minW;
                Console.Write("\nmin = " + (minW));
                foreach (var i in listIndex)
                {
                    Console.Write("\n" + graf[i].A + "->" + graf[i].B + ": " + graf[i].W);
                    graf[i].W = graf[i].W - minW;
                    Console.Write(" - " + minW + " = " + graf[i].W);
                }
                graf = graf.Where(x => x.W > 0).ToList();
                Console.Write("\n");
                //Console.Write("\nGraf:");
                //LookList(graf);
            }
        }

        private static void LookPut(List<int> put)
        {

            Console.Write("\n" + (put[0] + 1));
            for (int i = 1; i < put.Count; i++)
            {
                int p = put[i];
                Console.Write(", " + (p + 1));
            }
            Console.Write("\n");
        }

        private static List<int> SearchPutBFS(List<GrafModel> graf, int startNode, int endNode)
        {
            var a = graf.Select(x => x.A).Max();
            var b = graf.Select(x => x.B).Max();
            var n = a > b ? a : b;

            var puti = new List<List<int>>();
            for (int i = 0; i < n + 1; i++)
            {
                puti.Add(new List<int>() { startNode });
            }
            var q = new List<int>();
            var mark = InitIntList(n + 1);
            q.Add(startNode);
            mark[startNode] = 1;
            var id = 0;
            while (q.Count > id)
            {
                var v = q[id];
                id++;
                var listsosed = graf.Where(x => x.A == v && !q.Any(y => y == x.B)).ToList();
                for (int i = 0; i < listsosed.Count; i++)
                {
                    if (mark[listsosed[i].B] == 0)
                    {
                        mark[listsosed[i].B] = 1;
                        q.Add(listsosed[i].B);
                        puti[listsosed[i].B] = puti[v].ToList();
                        puti[listsosed[i].B].Add(listsosed[i].B);
                        if (listsosed[i].B == endNode)
                        {
                            return puti[listsosed[i].B];
                        }
                    }
                }
            }

            return null;
        }
        private static Tuple<bool, List<int>> SearchPutDFS(List<GrafModel> graf, int startNode, int endNode, List<int> mark)
        {
            if (startNode == endNode)
            {
                var puti = new List<int>() { endNode };
                return new Tuple<bool, List<int>>(true, puti);
            }
            mark[startNode] = 1;
            var listsosed = graf.Where(x => x.A == startNode && mark[x.B] == 0).ToList();
            var put = new List<int>();
            foreach (var l in listsosed)
            {
                var tuple = SearchPutDFS(graf, l.B, endNode, mark.ToList());
                if (tuple.Item1)
                {
                    var puti = tuple.Item2.ToList();
                    puti.Insert(0, startNode);
                    return new Tuple<bool, List<int>>(true, puti);
                }
            }
            return new Tuple<bool, List<int>>(false, null);
        }
        /*private static List<int> SearchPutDFS(List<GrafModel> graf, int startNode, int endNode)
        {
            var a = graf.Select(x => x.A).Max();
            var b = graf.Select(x => x.B).Max();
            var n = a > b ? a : b;

            var puti = new List<List<int>>();
            for (int i = 0; i < n + 1; i++)
            {
                puti.Add(new List<int>() { startNode });
            }

            var q = new List<int>();
            q.Add(startNode);
            var mark = InitIntList(n + 1);
            mark[startNode] = 1;
            var id = 0;
            while (q.Count > id)
            {
                var qtmp = q.ToList();
                var marktmp = mark.ToList();
                var putitmp = puti.ToList();
                var v = q[id];
                id++;
                var listsosed = graf.Where(x => x.A == v && !q.Any(y => y == x.B)).ToList();
                for (int i = 0; i < listsosed.Count; i++)
                {
                    if (mark[listsosed[i].B] == 0)
                    {
                        mark[listsosed[i].B] = 1;
                        q.Add(listsosed[i].B);
                        puti[listsosed[i].B] = puti[v].ToList();
                        puti[listsosed[i].B].Add(listsosed[i].B);
                        if (listsosed[i].B == endNode)
                        {
                            return puti[listsosed[i].B];
                        }
                    }
                }
            }
        }*/


        static List<int> InitIntList(int count)
        {
            var list = new List<int>();
            for (int i = 0; i < count; i++)
            {
                list.Add(0);
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
        static private void LookList(List<GrafModel> graf)
        {
            foreach (var item in graf)
            {
                Console.Write($"\n{item.A + 1}->{item.B + 1}={item.W}");
            }
        }
    }
}

//1   2   2
//1   3   4
//1   4   5
//2   6   4
//2   5   4
//3   5   3
//4   7   6
//6   12  5
//7   8   3
//7   13  4
//8   11  12
//9   10  5
//12  9   10
//12  11  6
//11  13  5