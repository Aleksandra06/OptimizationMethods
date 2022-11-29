using System;
using System.Collections.Generic;
using System.IO;

namespace Backpack_unlimited
{
    class Program
    {
        static int SIZE = 3;
        static int BACKPACK_WEIGHT = 2900000;
        static List<int> weight = new List<int>() { 3, 5, 8 };
        static List<int> price = new List<int>() { 8, 14, 23 };
        static void Main(string[] args)
        {
            if (BACKPACK_WEIGHT < 0)
            {
                Console.Write("ERROR");
            }

            int old_price, new_price;

            var table = InitMas(SIZE + 1, BACKPACK_WEIGHT + 1);

            for (int i = 0; i < SIZE + 1; i++)
            {
                for (int j = 0; j < BACKPACK_WEIGHT + 1; j++)
                {
                    if (i == 0 || j == 0)
                    {
                        table[i][j] = 0;
                    }
                    else if (weight[i - 1] <= j)
                    {
                        old_price = table[i - 1][j];
                        new_price = j - weight[i - 1] > 0 
                            ? (table[i - 1][j - weight[i - 1]] > table[i][j - weight[i - 1]] 
                            ? table[i - 1][j - weight[i - 1]] + price[i - 1] 
                            : table[i][j - weight[i - 1]] + price[i - 1]) 
                            : price[i - 1];
                        table[i][j] = old_price > new_price ? old_price : new_price;
                    }
                    else if (table[i - 1][j] > 0)
                    {
                        table[i][j] = table[i - 1][j];
                    }
                }
            }

            print_data(table);
            //print_table(table);
            //Console.Write($"\nmax value = {table[SIZE][BACKPACK_WEIGHT]}\n");

            var count_take = InitIntList(SIZE);

            for (int i = SIZE, j = BACKPACK_WEIGHT; ;)
            {
                if (table[i][j] == 0)
                {
                    break;
                }
                bool key = false;
                for (int k = j - 1; k >= 0; k--)
                {
                    if (table[i][j] - price[i - 1] == table[i][k])
                    {
                        count_take[i - 1]++;
                        j = k;
                        key = true;
                        break;
                    }
                }
                if (!key && table[i][j] - price[i - 2] == table[i - 1][j])
                {
                    count_take[i - 2]++;
                    i--;
                }
                else if (!key)
                {
                    if (table[i][j] == table[i][j - 1])
                        j--;
                    else if (table[i][j] == table[i - 1][j])
                        i--;
                    else
                    {
                        Console.Write("ERROR");
                    }
                }
            }

            Console.Write("\ncount: ");
            for (int i = 0; i < SIZE; i++)
                Console.Write($"{count_take[i]} ");
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
        static List<int> InitIntList(int count)
        {
            var list = new List<int>();
            for (int i = 0; i < count; i++)
            {
                list.Add(0);
            }
            return list;
        }
        static int find_length(int num)
        {
            int length;
            for (length = 1; num > 10; length++)
                num /= 10;

            return length;
        }
        static void print_data(List<List<int>> table)
        {
            Console.Write($"number items = {SIZE}");
            Console.Write($"\nbackpack weigth = {BACKPACK_WEIGHT}");
            Console.Write("\nWEIGHTS = ");
            for (int i = 0; i < SIZE; i++)
            {
                Console.Write($"|{weight[i]}|");
                for (int k = 0; k < 4 - find_length(weight[i]); k++)
                    Console.Write(" ");
            }
            Console.Write("\nprices = ");
            for (int i = 0; i < SIZE; i++)
            {
                Console.Write($"|{price[i]}| ");
                for (int k = 0; k < 4 - find_length(price[i]); k++)
                    Console.Write(" ");
            }
        }

        static void print_table(List<List<int>> table)
        {
            Console.Write("\ntable:\n");
            for (int i = 0; i < SIZE + 1; i++)
            {
                for (int j = 0; j < BACKPACK_WEIGHT + 1; j++)
                {
                    Console.Write($"{table[i][j]}");
                    for (int k = 0; k < 4 - find_length(table[i][j]); k++)
                        Console.Write(" ");
                }
                Console.Write("\n");
            }
        }

    }
}