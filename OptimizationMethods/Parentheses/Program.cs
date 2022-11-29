using System;
using System.Collections.Generic;
using System.IO;

namespace Kraskall
{
    class Program
    {

        static int INFINITY = 90000;
        static int NUMBER_OF_MATRICES = 4;
        static List<int> matrices = new List<int>() { 10, 20, 50, 1, 100 };

        static void Main(string[] args)
        {
            int i, j, M;
            int n = NUMBER_OF_MATRICES;
            List<List<int>> matrix = new List<List<int>>();
            for (i = 0; i < n; i++)
            {
                matrix.Add( new List<int>());
                matrix[i].Add(matrices[i]);
                matrix[i].Add(matrices[i+1]);
            }



            List<List<int>> ms = InitMas(NUMBER_OF_MATRICES, NUMBER_OF_MATRICES);

            for (i = 1; i <= NUMBER_OF_MATRICES; i++)
            {
                Console.Write($"[%{matrices[i - 1]}:{matrices[i]}]");
            }

            //for (i = 0; i < n; i++)
            //{
            //    Console.Write(" [" + matrix[i][0] + "x" + matrix[i][1] + "]");
            //}

            Console.Write("\n");
            var r = InitIntList(n + 1);
            var mass = InitIntList(n + 1);
            for (i = 0; i < n; i++)
            {
                mass[i] = matrix[i][0];
            }
            mass[n] = matrix[n - 1][1];

            for (i = 0; i < n + 1; ++i)
                r[i] = mass[i];

            var f = InitMas(n, n);

            for (i = 0; i < n; ++i)
            {
                //f[i] = new int[n];
                for (j = 0; j < n; ++j)
                    f[i][j] = -1;
            }
            for (i = 0; i < n; ++i)
                f[i][i] = 0;

            int t, k, Temp;
            var jm = InitMas(n, n);

            for (i = 0; i < n; ++i)
            {
                //jm[i] = new int[n];
                for (j = 0; j < n; ++j)
                    jm[i][j] = -1;
            }
            int g = 0;
            //Console.Write("\n");
            //for (i = 0; i < n; i++)
            //{
            //    Console.WriteLine(" f(" + i + "," + i + ")=" + g);
            //}

            for (t = 1; t < n; t++)
            {
                for (k = 0; k < n - t; k++)
                {
                    jm[k][k + t] = k;
                    f[k][k + t] = f[k][k] + f[k + 1][k + t] + r[k] * r[k + 1] * r[k + t + 1];
                   // Console.WriteLine(" f(" + (k + 1) + "," + (k + t + 1) + ") = min(\n\tf[" + (k + 1) + "," + (k + 1) + "] + f[" + (k + 2) + ", " + (k + t + 1) + "] + " + r[k] + " * " + r[k + 1] + " * " + r[k + t + 1]);
                    for (j = k + 1; j < k + t; j++)
                    {
                        //Console.WriteLine(";\n" + "\tf[" + (k + 1) + "," + (j + 1) + "] + f[" + (j + 2) + ", " + (k + t + 1) + "] + " + r[k] + " * " + r[j + 1] + " * " + r[k + t + 1]);
                        Temp = f[k][j] + f[j + 1][k + t] + r[k] * r[j + 1] * r[k + t + 1];
                        if (Temp <= f[k][k + t])
                        {
                            jm[k][k + t] = j;
                            f[k][k + t] = Temp;
                        }
                    }
                   // Console.WriteLine(")\n\t=  f[" + (k + 1) + "," + j + "]+ f[" + (j + 1) + "," + (k + t + 1) + "] + " + r[k] + " * " + r[j] + " * " + r[k + t + 1] + " = " + f[k][k + t]);
                }
            }

            Console.Write("\n");

            //Console.Write("\n");
            //for (i = 0; i < n; ++i)
            //{
            //    for (j = 0; j < n; ++j)
            //        //cout << " f[" << i + 1 << "," << j + 1 << "] = " << setw(5) << f[i][j] << setw(4) << "   ";
            //        Console.WriteLine(f[i][j] + " ");
            //    Console.Write("\n");
            //}

            Console.Write("\n");
            Print(f, jm, n, 0, n - 1);
            Console.Write("\n");
        }
        static void Print(List<List<int>> f, List<List<int>> jm, int n, int Up, int Down)
        {
            switch (Down - Up)
            {
                case 0:
                    {
                        Console.Write(" M[" + Up + 1 + "] ");
                        return;
                    }
                case 1:
                    {
                        Console.Write("( ");
                        Console.Write("M[" + (Up + 1) + "] * M[" + (Down + 1) + "]");
                        Console.Write(" )");
                        return;
                    }
                default:
                    {
                        Console.Write("( ");
                        Print(f, jm, n, Up, jm[Up][Down]);
                        Print(f, jm, n, jm[Up][Down] + 1, Down);
                        Console.Write(" )");
                        return;
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
        //static int INFINITY = 90000;
        //static int NUMBER_OF_MATRICES = 9;
        //static List<int> matrices = new List<int>(){ 1, 2, 9, 4, 2, 5, 2, 6, 3, 9 };
        //static void Main(string[] args)
        //{
        //    List<List<int>> ms = InitMas(NUMBER_OF_MATRICES, NUMBER_OF_MATRICES);

        //    int rd = 0;
        //    int r = optimal_braces(1, NUMBER_OF_MATRICES, ms);
        //    for (int i = 1; i <= NUMBER_OF_MATRICES; i++)
        //    {
        //        Console.Write($"[%{matrices[i - 1]}:{matrices[i]}]");
        //    }
        //    Console.Write($"\nRESULT:{r}d\n");

        //    for (int i = 0; i < NUMBER_OF_MATRICES; i++)
        //    {
        //        for (int j = 0; j < NUMBER_OF_MATRICES; j++)
        //            Console.Write(ms[i][j] + " ");
        //        Console.Write("\n");
        //    }

        //    var open_braces = InitIntList(NUMBER_OF_MATRICES);
        //    var close_braces = InitIntList(NUMBER_OF_MATRICES);

        //    func_brace(ms, open_braces, close_braces, 0, NUMBER_OF_MATRICES);

        //    /*for (int i = 0; i < NUMBER_OF_MATRICES; i++) {
        //      printf("%d:%d ", open_braces[i], close_braces[i]);
        //    }*/
        //    Console.Write("\n\n");

        //    for (int i = 0; i < NUMBER_OF_MATRICES; i++)
        //    {
        //        while (open_braces[i] > 0)
        //        {
        //            Console.Write("(");
        //            open_braces[i]--;
        //        }
        //        Console.Write($"A_{i + 1}");

        //        while (close_braces[i] > 0)
        //        {
        //            Console.Write(")");
        //            close_braces[i]--;
        //        }
        //        if (i + 1 != NUMBER_OF_MATRICES)
        //            Console.Write(" * ");
        //    }
        //}
        //static List<List<int>> InitMas(int n, int m)
        //{
        //    var mass = new List<List<int>>();
        //    for (int i = 0; i < n; i++)
        //    {
        //        var tmp = new List<int>();
        //        for (int j = 0; j < m; j++)
        //        {
        //            tmp.Add(0);
        //        }
        //        mass.Add(tmp);
        //    }

        //    return mass;
        //}
        //static List<int> InitIntList(int count)
        //{
        //    var list = new List<int>();
        //    for (int i = 0; i < count; i++)
        //    {
        //        list.Add(0);
        //    }
        //    return list;
        //}
        //static int optimal_braces(int i, int j, List<List<int>> ms)
        //{
        //    int min_result = INFINITY, result;
        //    if (i < j)
        //    {
        //        for (int k = i; k < j; k++)
        //        {
        //            result = optimal_braces(i, k, ms) + optimal_braces(k + 1, j, ms) + matrices[i - 1] * matrices[k] * matrices[j];
        //            if (result < min_result)
        //            {
        //                min_result = result;
        //                ms[i - 1][j - 1] = k;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        min_result = 0;
        //    }

        //    return min_result;
        //}

        //static void func_brace(List<List<int>> ms, List<int> open_braces, List<int> close_braces, int begin, int end)
        //{
        //    //printf("%d:%d\n", begin, end);
        //    for (int point = begin + 1; point < end; point++)
        //    {
        //        if (close_braces[begin] == 0)
        //        {
        //            open_braces[begin]++;
        //            if (point + 1 < end && ms[begin][point] != ms[begin][point + 1] || point + 1 == end)
        //            {
        //                close_braces[point]++;
        //            }
        //            else
        //            {
        //                int start, last;
        //                for (start = point, last = point + 1; last < end; last++)
        //                {
        //                    if (ms[begin][start] != ms[begin][last])
        //                    {
        //                        break;
        //                    }
        //                }
        //                func_brace(ms, open_braces, close_braces, start, last);
        //                point = last - 1;
        //                close_braces[point]++;
        //            }
        //        }
        //    }
        //}
    }
}