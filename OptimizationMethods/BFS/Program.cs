using System.Security.Cryptography.X509Certificates;

namespace BFS
{
    public class Program
    {
        public class GrafModel
        {
            public int A { get; set; }//из
            public int B { get; set; }//в
            public int W { get; set; }//вec
        }
        static void Main(string[] args)
        {
            var path = "graf.txt";
            var data = File.ReadAllLines(path);
            var graf = GenerationGraf();
             AddNode(graf, 2, 3009, 2);
             AddNode(graf, 2003, 2019, 13);
            //var graf = new Dictionary<int, List<GrafModel>>();
            //foreach (var row in data)
            //{
            //    var tmp = new List<int>();
            //    var col = row.Split("\t");
            //    foreach (var item in col)
            //    {
            //        tmp.Add(int.Parse(item));
            //    }
            //    if (!graf.ContainsKey(tmp[0] - 1))
            //    {
            //        graf.Add(tmp[0]-1, new List<GrafModel>() { new GrafModel() { A = tmp[0] - 1, B = tmp[1]-1, W = tmp[2] } });
            //    }
            //    else
            //    {
            //        var list = graf[tmp[0] - 1];
            //        list.Add(new GrafModel() { A = tmp[0]-1, B = tmp[1] - 1 , W = tmp[2] });
            //        graf[tmp[0]-1] = list;
            //    }
            //}
            var startNode = 0;
            var endNode = 999999;
            Run(graf, startNode, endNode);
        }

        private static void AddNode(Dictionary<int, List<GrafModel>> graf, int a, int b, int w)
        {
            if (!graf.ContainsKey(a))
            {
                graf.Add(a, new List<GrafModel>() { new GrafModel() { A = a, B = b, W = w } });
            }
            else
            {
                var list = graf[a];
                list.Add(new GrafModel() { A = a, B = b, W = w });
                graf[a] = list;
            }
        }

        private static Dictionary<int, List<GrafModel>> GenerationGraf()
        {
            var graf = new Dictionary<int, List<GrafModel>>();
            var rows = 1000;
            var cols = 1000;
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    GrafModel tmp = null;
                    var item = (row * cols) + col;
                    if (col + 1 < cols)
                    {
                        tmp = new GrafModel() { A = item, B = item + 1, W = 1 };
                    }
                    if (tmp != null)
                    {
                        if (!graf.ContainsKey(tmp.A))
                        {
                            graf.Add(tmp.A, new List<GrafModel>() { tmp });
                        }
                        else
                        {
                            var list = graf[tmp.A];
                            list.Add(tmp);
                            graf[tmp.A] = list;
                        }
                    }
                    tmp = null;
                    if (row + 1 < rows)
                    {
                        tmp = new GrafModel() { A = item, B = item + cols, W = 1 };
                    }
                    if (tmp != null)
                    {
                        if (!graf.ContainsKey(tmp.A))
                        {
                            graf.Add(tmp.A, new List<GrafModel>() { tmp });
                        }
                        else
                        {
                            var list = graf[tmp.A];
                            list.Add(tmp);
                            graf[tmp.A] = list;
                        }
                    }
                }
            }
            return graf;
        }
        private static void Run(Dictionary<int, List<GrafModel>> graf, int startNode, int endNode)//для 1000*1000
        {
            var n = 1000 * 1000;

            var puti = new Dictionary<int, int>();
            for (int i = 0; i < n; i++)
            {
                puti.Add(i, -1);
            }
            puti[startNode] = startNode;
            Queue<int> q = new Queue<int>();
            var mark = InitBoolList(n);
            var d = InitIntList(n);
            q.Enqueue(startNode);
            d[startNode] = 0;
            mark[startNode] = true;
            var id = 0;
            //var sloys = new List<List<int>>() { new List<int>() { startNode } };
            //sloys.Add(new List<int>());
            //var sloysId = 1;
            //Console.Write("\n" + sloysId + ": " + (startNode + 1));
            var flag = false;
            while (q.Count > 0 && !flag)
            {
                if (id % 100000 == 0)
                {
                    //  Console.WriteLine(id);
                }
                var v = q.Dequeue();
                //Console.Write("\n" + (v + 1) + ":");
                id++;
                var listsosed = graf.ContainsKey(v) ? graf[v].ToList() : null;
                if (listsosed == null)
                {
                    continue;
                }
                for (int i = 0; i < listsosed.Count; i++)
                {
                    if (mark[listsosed[i].B] == false || (d[v] + listsosed[i].W) < d[listsosed[i].B])
                    {
                        d[listsosed[i].B] = d[v] + listsosed[i].W;
                        mark[listsosed[i].B] = true;
                        q.Enqueue(listsosed[i].B);
                        puti[listsosed[i].B] = v;
                        //if (listsosed[i].B == endNode)
                        //{
                        //    flag = true;
                        //}
                        //sloys.Last().Add(listsosed[i].B);
                        //Console.Write(", " + (q.Last() + 1));
                    }
                }
                //q.Remove(v);
                //if (v == sloys[sloysId - 1].LastOrDefault())
                //{
                //    //if (sloys.Last().Count() > 0)
                //    //{
                //    //    Console.Write("\n" + (sloys.Count()) + ": ");
                //    //}
                //    //foreach (var l in sloys.Last())
                //    //{
                //    //    Console.Write(", " + (l + 1));
                //    //}
                //    sloys.Add(new List<int>());
                //    sloysId++;
                //}
            }


            //Console.Write("\n\n");
            //for (int i = 0; i < d.Count; i++)
            //{
            //    Console.WriteLine((i + 1) + "=" + d[i]);
            //}
            //Console.Write("\n\n");
            //for (int i = 0; i < q.Count; i++)
            //{
            //    Console.Write(", " + (q[i] + 1));
            //}
            //Console.Write("\n");

            var put = new List<int>();
            Console.Write("\n" + (endNode + 1) + ":");
            var node = endNode;
            while (node != startNode)
            {
                put.Add(node);
                node = puti[put.Last()];
            }
            put.Add(startNode);
            put.Reverse();
            var s = 0;
            for (int i = 0; i < put.Count; i++)
            {
                Console.Write(", " + (put[i] + 1));
                s++;
            }
            Console.Write("\nСтоимость " + d[endNode]);
        }
        private static void Run(List<GrafModel> graf, int startNode)
        {
            var a = graf.Select(x => x.A).Max();
            var b = graf.Select(x => x.B).Max();
            var n = a > b ? a : b;

            //var puti = new List<List<int>>();
            //for (int i = 0; i < n + 1; i++)
            //{
            //    puti.Add(new List<int>() { startNode });
            //}
            var q = new List<int>();
            var mark = InitIntList(n + 1);
            var d = InitIntList(n + 1);
            q.Add(startNode);
            d[startNode] = 0;
            mark[startNode] = 1;
            var id = 0;
            var sloys = new List<List<int>>() { new List<int>() { startNode } };
            sloys.Add(new List<int>());
            var sloysId = 1;
            //Console.Write("\n" + sloysId + ": " + (startNode + 1));
            while (q.Count > id)
            {
                if (id % 100 == 0)
                {
                    Console.WriteLine(id);
                }
                var v = q[id];
                //Console.Write("\n" + (v + 1) + ":");
                id++;
                var listsosed = graf.Where(x => x.A == v && !q.Any(y => y == x.B)).ToList();
                for (int i = 0; i < listsosed.Count; i++)
                {
                    if (mark[listsosed[i].B] == 0)
                    {
                        d[listsosed[i].B] = d[v] + 1;
                        mark[listsosed[i].B] = 1;
                        q.Add(listsosed[i].B);
                        //puti[listsosed[i].B] = puti[v].ToList();
                        //puti[listsosed[i].B].Add(listsosed[i].B);
                        sloys.Last().Add(listsosed[i].B);
                        //Console.Write(", " + (q.Last() + 1));
                    }
                }
                if (v == sloys[sloysId - 1].LastOrDefault())
                {
                    //if (sloys.Last().Count() > 0)
                    //{
                    //    Console.Write("\n" + (sloys.Count()) + ": ");
                    //}
                    //foreach (var l in sloys.Last())
                    //{
                    //    Console.Write(", " + (l + 1));
                    //}
                    sloys.Add(new List<int>());
                    sloysId++;
                }
            }

            Console.Write("\n\n");
            for (int i = 0; i < d.Count; i++)
            {
                Console.WriteLine((i + 1) + "=" + d[i]);
            }
            Console.Write("\n\n");
            for (int i = 0; i < q.Count; i++)
            {
                Console.Write(", " + (q[i] + 1));
            }
            Console.Write("\n");

            //int putid = 1;
            //foreach (var put in puti)
            //{
            //    Console.Write("\n" + putid + ":");
            //    putid++;
            //    for (int i = 0; i < put.Count; i++)
            //    {
            //        Console.Write(", " + (put[i] + 1));
            //    }
            //}
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