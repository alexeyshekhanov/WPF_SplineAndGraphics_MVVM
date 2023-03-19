using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public struct SplineDataItem
    {
        public double coordinate { get; set; }
        public double value { get; set; }
        public double valueOfFirstDerivative { get; set; }
        public double valueOfSecondDerivative { get; set; }

        public SplineDataItem(double coordinate, double value, double valueOfFirstDerivative, double valueOfSecondDerivative)
        {
            this.coordinate = coordinate;
            this.value = value;
            this.valueOfFirstDerivative = valueOfFirstDerivative;
            this.valueOfSecondDerivative = valueOfSecondDerivative;
        }
        
        public string ToString(string format)
        {
            return $"Coordinate: {string.Format(format, coordinate)}, " + "\n" +
                $"Value: {string.Format(format, value)} " + "\n" +
                $"Value of the first derivative:  {string.Format(format, valueOfFirstDerivative)}" + "\n" +
                $"Value of the second derivative: {string.Format(format, valueOfSecondDerivative)}" + "\n";
        }

        public override string ToString()
        {
            return $"Coordinate: {coordinate}, " + "\n" +
                $"Value: {value} " + "\n" +
                $"Value of the first derivative:  {valueOfFirstDerivative}" + "\n" +
                $"Value of the second derivative: {valueOfSecondDerivative}" + "\n";
        }
    }
}
