﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Functions
    {
        public static double cube(double x)
        {
            return x * x * x;
        }

        public static double linear(double x)
        {
            return x;
        }
        public static double random(double x)
        {
            var rnd = new Random();
            return rnd.NextDouble();
        }
    }
}
