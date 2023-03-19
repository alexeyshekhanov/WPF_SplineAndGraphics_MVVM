using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
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
            var rnd = new Random((int)x);
            return rnd.NextDouble();
        }
    }
}
