using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Stairs
{
    class Program
    {
        static void Main(string[] args)
        {
            Lepus();
        }
        /// <summary>
        /// Лесенка
        /// </summary>
        static void Ladder()
        {
            var pathIn = "ladder.in";
            var pathOut = "ladder.out";
            var data = File.ReadAllLines(pathIn);
            var count = int.Parse(data[0]);
            var mas = new List<int>();
            var list = data[1].Split(" ");
            mas.Add(0);
            foreach (var item in list)
            {
                mas.Add(int.Parse(item));
            }
            var d = new List<int>();
            var number = 0;
            for (int s = 2; s <= count; s++)
            {
                var max = 0;
                if (mas[s - 2] > mas[s - 1])
                {
                    max = mas[s - 2];
                    d.Add(s - 2);
                }
                else
                {
                    max = mas[s - 1];
                    d.Add(s - 1);
                }
                mas[s] = max + mas[s];
            }
            var result = mas[count];
            Console.WriteLine(result);
            StreamWriter f = new StreamWriter(pathOut);
            f.WriteLine(result);
            f.Close();
        }
        /// <summary>
        /// Зайчик
        /// </summary>
        public class Way
        {
            public char v;
            public int count;
        }
        static void Lepus()
        {
            var pathIn = "lepus.in";
            var pathOut = "lepus.out";
            var data = File.ReadAllLines(pathIn);
            var count = int.Parse(data[0]);
            var list = data[1];
            var mas = new List<Way>();
            for (int i = 0; i < count; i++)
            {
                mas.Add(new Way() { v = list[i], count = 0 });
            }
            var result = 0;
            for (int i = 1; i < count; i++)
            {
                if (mas[i].v == 119)
                {
                    continue;
                }
                var max = mas[i - 1].v == 119 && mas[i - 1].count != -1 ? -1 : mas[i - 1].count;
                if (i - 3 >= 0)
                {
                    if (mas[i - 3].count > max && mas[i - 3].v != 119 && mas[i - 3].count != -1)
                    {
                        max = mas[i - 3].count;
                    }
                }
                if (i - 5 >= 0)
                {
                    if (mas[i - 5].count > max && mas[i - 5].v != 119 && mas[i - 5].count != -1)
                    {
                        max = mas[i - 5].count;
                    }
                }
                if (max == -1)
                {
                    mas[i].count = -1;
                    continue;
                }

                mas[i].count = max + (mas[i].v == 34 ? 1 : 0);
            }
            result = mas[count - 1].count;
            Console.WriteLine(result);
            StreamWriter f = new StreamWriter(pathOut);
            f.WriteLine(result);
            f.Close();
        }
        /// <summary>
        /// Ход конем
        /// </summary>
        static void Knight()
        {
            var pathIn = "knight.in";
            var pathOut = "knight.out";
            var data = File.ReadAllLines(pathIn);
            var tmp = data[0].Split(" ");
            var rows = int.Parse(tmp[0]);
            var cols = int.Parse(tmp[1]);
            List<List<int>> mas = new List<List<int>>();
            for (int row = 0; row < rows; row++)
            {
                mas.Add(new List<int>());
                mas[row] = new List<int>();
                for (int col = 0; col < cols; col++)
                {
                    mas[row].Add(0);
                }
            }
            for (int row = 1; row < rows; row++)
            {
                for (int col = 1; col < cols; col++)
                {
                    var count = 0;
                    if (col - 1 > 0 && row - 2 > 0)
                    {
                        if (mas[row - 2][col - 1] != 0)
                        {
                            count = mas[row - 2][col - 1];
                        }
                    }
                    else if (row - 2 == 0 && col - 1 == 0)
                    {
                        count = 1;
                    }
                    if (col - 2 > 0 && row - 1 > 0)
                    {
                        if (mas[row - 1][col - 2] != 0)
                        {
                            count += mas[row - 1][col - 2];
                        }
                    }
                    else if (row - 1 == 0 && col - 2 == 0)
                    {
                        count = 1;
                    }
                    mas[row][col] = count;
                }
            }
            //var result = mas[rows - 1][cols - 1];
            //Console.WriteLine(result);
            //StreamWriter f = new StreamWriter(pathOut);
            //f.WriteLine(result);
            //f.Close();
        }
        /// <summary>
        /// Стоимость маршрута
        /// </summary>
        static void King2()
        {
            var pathIn = "king2.in";
            var pathOut = "king2.out";
            var data = File.ReadAllLines(pathIn);
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
            var n = graf.Count;
            for (int row = n - 1; row >= 0; row--)
            {
                for (int col = 0; col < n; col++)
                {
                    if(col == 0 && row == n - 1)
                    {
                        continue;
                    }
                    var min = int.MaxValue;
                    if (row < n - 1)
                    {
                        min = graf[row + 1][col];
                    }
                    if (col > 0)
                    {
                        if (min > graf[row][col - 1])
                        {
                            min = graf[row][col - 1];
                        }
                    }
                    if (col > 0 && row < n - 1)
                    {
                        if (min > graf[row + 1][col - 1])
                        {
                            min = graf[row + 1][col - 1];
                        }
                    }
                    graf[row][col] += min;
                }
                PrintGraf(graf);
            }
            var result = graf[0][n - 1];
            Console.WriteLine(result);
            StreamWriter f = new StreamWriter(pathOut);
            f.WriteLine(result);
            f.Close();
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
                    Console.Write("\t" + C[i][j]);
                }
                Console.Write("\n");
            }
        }
    }
}
//18
//-2, -3, -4, -1, -3, -4, -3, -2, 0, -3, -2, -1, 2, 1, 0, -3, -4, 1