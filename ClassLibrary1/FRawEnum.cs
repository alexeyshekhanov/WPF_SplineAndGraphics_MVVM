using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public enum FRawEnum
    {
        cube = 0,
        linear = 1,
        random = 2
    }

    public delegate double FRaw(double x);
}
