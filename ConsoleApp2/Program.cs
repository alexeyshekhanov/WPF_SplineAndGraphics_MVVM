// See https://aka.ms/new-console-template for more information
using ConsoleApp21;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.Security;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Prac1
{
    class Program
    {
        static void Main(string[] args)
        {
            RawData rawData = new RawData(2, 10, 5, true, x => x * x * x);
            rawData.initializeTheValues();
            SplineData splineData = new SplineData(rawData, 12, 60, 6, new double[] { 0, 2, 4, 6, 8, 10 });
            splineData.splineConstruction();
            foreach (var item in splineData.calculatedSplineValues)
            {
                Console.WriteLine(item);
            }

        }
    }
}


