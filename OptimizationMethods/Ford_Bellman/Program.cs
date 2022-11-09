using Ford_Bellman;
using System;
using System.Collections.Generic;
using System.IO;

namespace Ford_bellman
{
    class Program
    {
        static readonly int inf = -1;
        static void Main(string[] args)
        {
            (new FordBellmanMatrix()).Run();
            //(new FordBellmanList()).Run();
        }
    }
}