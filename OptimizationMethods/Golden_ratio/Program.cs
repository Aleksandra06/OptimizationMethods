using System;
using System.Collections.Generic;
using System.IO;

namespace Kraskall
{
    class Program
    {
        static double EDS = 0.000001;
        static double a = 1;
        static double b = 10;
        static double golden_ratio = (1 + Math.Sqrt(5)) / 2;

        static void Main(string[] args)
        {
            double x1, x2, y1, y2;

            x1 = b - (b - a) / golden_ratio;
            x2 = a + (b - a) / golden_ratio;
            y1 = Math.Sin(x1);
            y2 = Math.Sin(x2);

            while (Math.Abs(b - a) > EDS)
            {
                if (y1 <= y2)
                {
                    a = x1;
                    x1 = x2;
                    x2 = a + (b - a) / golden_ratio;
                    y1 = y2;
                    y2 = Math.Sin(x2);
                }
                else
                {
                    b = x2;
                    x2 = x1;
                    x1 = b - (b - a) / golden_ratio;
                    y2 = y1;
                    y1 = Math.Sin(x1);
                }
            }
            Console.WriteLine($"MAXIMUM X = {(a + b) / 2}\nF(X) = {Math.Sin((a + b) / 2)}");
        }

    }
}