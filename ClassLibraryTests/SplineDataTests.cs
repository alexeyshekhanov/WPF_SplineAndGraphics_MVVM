using ConsoleApp1;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace ClassLibraryTests
{
    public class SplineDataTests
    {
        [Fact]
        public void splineConstructionTest()
        {
            var rawData = new RawData(2, 6, 3, true, Functions.cube, new double[] {2, 4, 6}, new double[] {8, 64, 216});
            var splineData = new SplineData(rawData, 12, 36, 5);
            splineData.splineConstruction();

            var coordinates = new double[] { 2, 3, 4, 5, 6 };
            var values = new double[] { 8, 27, 64, 125, 216 };
            var valuesOfFirstDerivative = new double[] { 12, 27, 48, 75, 108 };
            var valuesOfSecondDerivative = new double[] { 12, 18, 24, 30, 36 };

            int precision = 5;

            Assert.Equal(320, splineData.valueOfIntegral, 5);
            for (int i = 0; i < splineData.numberOfNodesToCalculateValues; i++)
            {
                Assert.Equal(values[i], splineData.calculatedSplineValues[i].value, precision);
                Assert.Equal(coordinates[i], splineData.calculatedSplineValues[i].coordinate, precision);
                Assert.Equal(valuesOfFirstDerivative[i], splineData.calculatedSplineValues[i].valueOfFirstDerivative, precision);
                Assert.Equal(valuesOfSecondDerivative[i], splineData.calculatedSplineValues[i].valueOfSecondDerivative, precision);
            }
        }

        [Fact]
        public void splineConstructionExceptionTest()
        {
            var exeption = Assert.Throws<IndexOutOfRangeException>(() =>
            {
                var rawData = new RawData(2, 6, 0, true, Functions.cube);
                var splineData = new SplineData(rawData, 12, 36, 5);
                splineData.splineConstruction();
            });
        }
    }
}
