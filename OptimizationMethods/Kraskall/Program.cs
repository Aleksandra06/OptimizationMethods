using System;
using System.Collections.Generic;
using System.IO;

namespace Kraskall
{
    class Program
    {
        class Ostov
        {
            public int x { get; set; }
            public int y { get; set; }
            public int w { get; set; }
        }
        class List
        {
            public List<Ostov> ost { get; set; }
            public List<int> next { get; set; }
        }
        static readonly int inf = -1;
        static void Main(string[] args)
        {
            var path = "input-lab_search_min_skeleton_Kraskall.txt";
            var data = File.ReadAllLines(path);
            var graf = new List<List<int>>();
            foreach (var row in data)
            {
                Console.WriteLine($"{row}");
                var tmp = new List<int>();
                var col = row.Split(" ");
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
            PrintGraf(graf);
            Run(graf);
        }

        private static void Run(List<List<int>> matrix)
        {
            var N = matrix.Count;
            int triangle = 1;
            Console.WriteLine("top/bottom (1/2): " + triangle);
            int j1, j2;
            int min = 1;
            bool flag = true;
            List<Ostov> mOstov = new List<Ostov>();
            var l = mOstov;
            var mas = InitIntList(N);
            for (int i = 0; i < N; i++)
                mas[i] = i + 1;
            while (flag)
            {
                for (int i = 0; i < N; i++)
                {
                    if (triangle == 1)
                    {
                        j1 = i; j2 = N;
                    }
                    else
                    {
                        j1 = 0; j2 = i;
                    }
                    for (int j = j1; j < j2; j++)
                    {
                        if (matrix[i][j] == min)
                        {
                            //if (mas[i] != mas[j])
                            //{
                            //    if (mOstov->ost == NULL)
                            //    {
                            //        mOstov->ost = new ostov();
                            //        mOstov->ost->x = i;
                            //        mOstov->ost->y = j;
                            //        mOstov->ost->w = matrix[i][j];
                            //        l = mOstov;
                            //    }
                            //    else
                            //    {
                            //        l = l->next;
                            //        l = new list();
                            //        l->ost = new ostov();
                            //        l->next = NULL;
                            //        l->ost->x = i;
                            //        l->ost->y = j;
                            //        l->ost->w = matrix[i][j];
                            //    }
                            //}
                            Console.Write("(" + (i + 1) + "," + (j + 1) + ") = " + matrix[i][j]);
                            if (mas[i] == mas[j]) Console.Write(" - no");
                            Console.Write("\n");
                            if (mas[i] != mas[j])
                            {
                                int mn;
                                int mm;
                                if (mas[i] < mas[j]) { mn = mas[i]; mm = mas[j]; }
                                else { mn = mas[j]; mm = mas[i]; }
                                for (int k = 0; k < N; k++)
                                {
                                    if (mas[k] == mm || mas[k] == mn)
                                    {
                                        mas[k] = mn;
                                    }
                                }
                                flag = prov(mas);
                                if (flag == false) return;
                            }
                        }
                    }
                }
                min++;
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
        static bool prov(List<int> mas)
        {
            bool flg = false;
            for (int i = 0; i < mas.Count - 1; i++)
            {
                if (mas[i] != mas[i + 1])
                {
                    flg = true;
                }
                Console.Write(mas[i] + " ");
            }
            Console.Write(mas[6] + "\n");
            return flg;
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