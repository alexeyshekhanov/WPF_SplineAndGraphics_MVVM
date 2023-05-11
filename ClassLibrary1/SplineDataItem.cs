using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public struct SplineDataItem
    {
        public double Coordinate { get; set; }
        public double Value { get; set; }
        public double ValueOfFirstDerivative { get; set; }
        public double ValueOfSecondDerivative { get; set; }

        public SplineDataItem(double coordinate, double value, double valueOfFirstDerivative, double valueOfSecondDerivative)
        {
            this.Coordinate = coordinate;
            this.Value = value;
            this.ValueOfFirstDerivative = valueOfFirstDerivative;
            this.ValueOfSecondDerivative = valueOfSecondDerivative;
        }
    }
}
