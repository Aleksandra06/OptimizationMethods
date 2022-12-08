using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Gradient
{
    class Program
    {
        static double EPS = 0.000001;
        static double golden_ratio = (1 + Math.Sqrt(5)) / 2;
        class Coordinates
        {
            public double x;
            public double y;
        };
        static void Main(string[] args)
        {
            Coordinates original = new Coordinates();
            original.x = 3;
            original.y = 10;
            original = gradient_descent(original);
            Console.WriteLine($"Result: x = {original.x} y = {original.y}");
        }
        static double function(Coordinates coord)
        {
            return (coord.x - coord.y * coord.y) * (coord.x - coord.y * coord.y) + (coord.x - 0.5) * (coord.x - 0.5);
        }
        static Coordinates gradient(Coordinates coord)
        {
            Coordinates grad = new Coordinates();

            grad.x = 4 * coord.x - 2 * coord.y * coord.y - 1;
            grad.y = -4 * coord.x * coord.y + 4 * coord.y * coord.y * coord.y;

            return grad;
        }

        static Coordinates calculate(Coordinates current, Coordinates grad, double lambda)
        {
            Coordinates buffer = new Coordinates();

            buffer.x = current.x - lambda * grad.x;
            buffer.y = current.y - lambda * grad.y;

            return buffer;
        }

        static double golden_selection(double a, double b, Coordinates grad, Coordinates current)
        {
            double x1, x2;
            double y1, y2;

            x1 = b - (b - a) / golden_ratio;
            x2 = a + (b - a) / golden_ratio;
            y1 = function(calculate(current, grad, x1));
            y2 = function(calculate(current, grad, x2));
            while (Math.Abs(b - a) > EPS)
            {
                if (y1 <= y2)
                {
                    b = x2;
                    x2 = x1;
                    x1 = b - (b - a) / golden_ratio;
                    y2 = y1;
                    y1 = function(calculate(current, grad, x1));
                }
                else
                {
                    a = x1;
                    x1 = x2;
                    x2 = a + (b - a) / golden_ratio;
                    y1 = y2;
                    y2 = function(calculate(current, grad, x2));
                }
            }

            return (a + b) / 2;
        }

        static Coordinates gradient_descent(Coordinates original)
        {
            Coordinates current = original;
            Coordinates last;
            double dx = 0, dy = 0;
            Console.WriteLine($"COORDINATES: [{current.x} : {current.y}] MINIMUM: {function(current)}");
            do
            {
                last = current;
                Coordinates grad = gradient(current);
                double lambda = golden_selection(0, 10, grad, current);
                current = calculate(current, grad, lambda);
                //cout << "COORDINATES: [" << current.x << ":" << current.y << "] GRAD: ["<< grad.x << ":" << grad.y << "] MINIMUM: " << function(current) << " ";
                //cout << "dx:dy [" << current.x - last.x << ":" << current.y - last.y << "] scalar: " << (current.x - last.x) * dx + (current.y - last.y) * dy << endl;
                Console.WriteLine($"dx:dy [{current.x - last.x} : {current.y - last.y}] dx:dx_last: [{dx} : {dy}] scalar: {(current.x - last.x) * dx + (current.y - last.y) * dy}");
                dx = current.x - last.x;
                dy = current.y - last.y;
            } while (Math.Abs(function(current) - function(last)) > EPS);

            return current;
        }
    }
}