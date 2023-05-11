using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Functions
    {
        public static double Cube(double x)
        {
            return x * x * x;
        }

        public static double Linear(double x)
        {
            return x;
        }
        public static double MyRandom(double x)
        {
            var rnd = new Random();
            return rnd.NextDouble();
        }
    }
}
