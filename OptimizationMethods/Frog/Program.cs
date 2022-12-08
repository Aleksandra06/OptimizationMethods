using System;
using System.Collections.Generic;
using System.IO;

namespace Kraskall
{
    class Program
    {

        static int INFINITY = 9000;
        static int MISSING = 9001;
        static int NUMBER_OF_ROCKS = 11;
        static List<int> ROCKS = new List<int>() { 1, 2, 3, 5, 7, 8, 9, 10, 11, 15, 16 };

        static void Main(string[] args)
        {
            int line = find_max_jump(ROCKS[NUMBER_OF_ROCKS - 1]);
            int column = ROCKS[NUMBER_OF_ROCKS - 1] + 1;

            var matrix = InitMas(line, column);

            for (int i = 0; i < line; i++)
                for (int j = 0, k = 0; j < column; j++)
                {
                    if (ROCKS[k] != j)
                        matrix[i][j] = MISSING;
                    else
                    {
                        matrix[i][j] = INFINITY;
                        k++;
                    }
                }

            matrix[0][0] = 0;
            print_matrix(matrix, line, column);

            frog_in(matrix, line);
            Console.Write("\nMIN_PATH:\n");
            print_matrix(matrix, line, column);

            frog_out(matrix, line, column);

            Console.Write("\nEND");
        }
        static int find_max_jump(int num)
        {
            int max_jump = 0;
            for (int i = 1, result = 0; result <= num; i++, max_jump++)
                result += i;

            return max_jump;
        }

        static int find_length(int num)
        {
            int length;
            for (length = 1; num > 10; length++)
                num /= 10;

            return length;
        }

        static void print_matrix(List<List<int>> matrix, int line, int column)
        {
            Console.Write("-1  ");
            for (int i = 0; i < column; i++)
            {
                Console.Write(i);
                for (int k = 0; k < 4 - find_length(i); k++)
                    Console.Write(" ");
            }
            Console.Write("\n");

            for (int i = 0; i < line; i++)
            {
                Console.Write(i);
                for (int k = 0; k < 4 - find_length(i); k++)
                    Console.Write(" ");
                for (int j = 0; j < column; j++)
                {
                    if (matrix[i][j] == INFINITY)
                        Console.Write("inf ");
                    else if (matrix[i][j] == MISSING)
                        Console.Write("|   ");
                    else
                    {
                        Console.Write(matrix[i][j]);
                        for (int k = 0; k < 4 - find_length(matrix[i][j]); k++)
                            Console.Write(" ");
                    }
                }
                Console.Write("\n");
            }
        }

        static void frog_in(List<List<int>> matrix, int line)
        {
            for (int k = 0, y = 1; k < NUMBER_OF_ROCKS; k++)
            {
                while (ROCKS[k] != y)
                    y++;

                for (int x = 1; x < line; x++)
                {
                    if (y - x >= 0)
                    {
                        if (matrix[x - 1][y - x] != INFINITY && matrix[x][y] > matrix[x - 1][y - x])
                            matrix[x][y] = matrix[x - 1][y - x] + 1;

                        if (matrix[x][y - x] != INFINITY && matrix[x][y] > matrix[x][y - x])
                            matrix[x][y] = matrix[x][y - x] + 1;

                        if (x + 1 < line)
                            if (matrix[x + 1][y - x] != INFINITY && matrix[x][y] > matrix[x + 1][y - x])
                                matrix[x][y] = matrix[x + 1][y - x] + 1;
                    }
                }
            }
        }

        static void frog_out(List<List<int>> matrix, int line, int column)
        {
            int y = column - 1, min_path = INFINITY, x_path = 0;

            for (int x = 1; x < line; x++)
                if (matrix[x][y] < min_path)
                {
                    min_path = matrix[x][y];
                    x_path = x;
                }

            if (min_path == INFINITY)
            {
                Console.Write("\nSHORTEST PATH NOT FOUND\n");

                return;
            }

            var path = InitIntList(min_path);

            for (int x = x_path, count = min_path - 1; ;)
            {
                //Console.Write("\nPATH: %d [%d-%d]", count + 1, y, x);
                if (y - x >= 0)
                {
                    if (matrix[x - 1][y - x] == count)
                    {
                        path[count] = y;
                        count--;
                        y = y - x;
                        x = x - 1;
                    }

                    if (matrix[x][y - x] == count)
                    {
                        path[count] = y;
                        count--;
                        y = y - x;
                    }

                    if (matrix[x + 1][y - x] == count)
                    {
                        path[count] = y;
                        count--;
                        y = y - x;
                        x = x + 1;
                    }
                    if (y == 0)
                        break;
                }
                else
                {
                    Console.Write("ERROR");

                    return;
                }
            }

            Console.Write($"\n\nJUMPS: {min_path}\nPATH: ");
            for (int i = 0; i < min_path; i++)
                Console.Write("%d ", path[i]);
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
    }
}