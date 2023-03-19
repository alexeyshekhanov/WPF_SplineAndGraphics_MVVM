using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class SplineData: IEnumerable<string>
    {
        public RawData gridForSpline;
        public int numberOfNodesToCalculateValues;
        public List<SplineDataItem> calculatedSplineValues;
        public double ?valueOfIntegral;

        public double valueOfSecondDerivativeInTheRightLimit;
        public double valueOfSecondDerivativeInTheLeftLimit;


        public SplineData(RawData gridForSpline, 
            double valueOfSecondDerivativeInTheLeftLimit, 
            double valueOfSecondDerivativeInTheRightLimit,
            int NumberOfNodesToCalculateValues)
        {
            this.gridForSpline = gridForSpline;
            this.numberOfNodesToCalculateValues = NumberOfNodesToCalculateValues;
            this.valueOfSecondDerivativeInTheLeftLimit = valueOfSecondDerivativeInTheLeftLimit;
            this.valueOfSecondDerivativeInTheRightLimit = valueOfSecondDerivativeInTheRightLimit;

            calculatedSplineValues = new List<SplineDataItem>();
            //var leftLimititem = new SplineDataItem(gridForSpline.nodesOfGrid[0], 
            //    gridForSpline.valuesInNodes[0],
            //    0, valueOfSecondDerivativeInTheLeftLimit);
            //var rightLimititem = new SplineDataItem(gridForSpline.nodesOfGrid[gridForSpline.numberOfNodes - 1], 
            //    gridForSpline.valuesInNodes[gridForSpline.numberOfNodes - 1],
            //    0, valueOfSecondDerivativeInTheLeftLimit);
            //calculatedSplineValues.Add(leftLimititem);
            //calculatedSplineValues.Add(rightLimititem);
        }

        public void splineConstruction()
        {
            int nx = gridForSpline.numberOfNodes;
            int ny = 1;
            var x = new double[gridForSpline.numberOfNodes];
            var y = new double[gridForSpline.numberOfNodes];
            for (int i = 0; i < gridForSpline.numberOfNodes; i++)
            {
                x[i] = gridForSpline.nodesOfGrid[i];
                y[i] = gridForSpline.valuesInNodes[i];
            }
            double[] bc = new double[] { valueOfSecondDerivativeInTheLeftLimit, valueOfSecondDerivativeInTheRightLimit };
            double[] scoeff = new double[ny * 4 * (nx - 1)];
            int nsite = numberOfNodesToCalculateValues;

            double[] newNodes = new double[numberOfNodesToCalculateValues];
            newNodes[0] = gridForSpline.leftLimitOfSegment;
            newNodes[numberOfNodesToCalculateValues - 1] = gridForSpline.rightLimitOfSegment;
            var stepOfTheGrid = (gridForSpline.rightLimitOfSegment - gridForSpline.leftLimitOfSegment) /
                (numberOfNodesToCalculateValues - 1);
            for (int i = 1; i < numberOfNodesToCalculateValues - 1; i++)
            {
                newNodes[i] = gridForSpline.leftLimitOfSegment + stepOfTheGrid * i;
            }

            double[] site = newNodes;
            int ndorder = 3;
            int[] dorder = new int[3] { 1, 1, 1 };
            var numberOfNonZeroElementsInDorder = 0;
            for (int i = 0; i < dorder.Length; i++)
            {
                if (dorder[i] != 0)
                {
                    numberOfNonZeroElementsInDorder++;
                }
            }
            double[] interpolationValues = new double[ny * numberOfNonZeroElementsInDorder * nsite];
            double[] llim = new double[1] { gridForSpline.leftLimitOfSegment };
            double[] rlim = new double[1] { gridForSpline.rightLimitOfSegment };
            double[] integrationValues = new double[ny];
            int ret = 0;

            try
            {
                Interpolation(nx, ny, x, y, bc, scoeff, nsite, site, ndorder, dorder,
                    interpolationValues, 1, llim, rlim, integrationValues, ref ret);
                if (ret != 0)
                {
                    throw new Exception("ERROR");
                }

                for (int i = 0; i < newNodes.Length; i++)
                {
                    var temp = new SplineDataItem(newNodes[i], 
                        interpolationValues[3 * i], 
                        interpolationValues[3 * i + 1], 
                        interpolationValues[3 * i + 2]);
                    //calculatedSplineValues.Insert(calculatedSplineValues.Count - 2, temp);
                    calculatedSplineValues.Add(temp);
                }

                valueOfIntegral = integrationValues[0];


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public string ToLongString(string format)
        {
            string str = $"Grid information: {gridForSpline.ToLongString(format)}" + "\n";
            str = str + "Number | Coordinate | Value | 1st derivative value | 2nd derivative value" + "\n";
            for (int i = 0; i < calculatedSplineValues.Count; i++)
            {
                str = str + $"   {i}:        " + $"{string.Format(format, calculatedSplineValues[i].coordinate)}            " +
                    $"{string.Format(format, calculatedSplineValues[i].value)}         " +
                    $"{string.Format(format, calculatedSplineValues[i].valueOfFirstDerivative)}                      " +
                    $"{string.Format(format, calculatedSplineValues[i].valueOfSecondDerivative)}" + "\n";
            }
            str = str + $"Value of the integral with limits {gridForSpline.leftLimitOfSegment}, " +
                $"{gridForSpline.rightLimitOfSegment}: " + $"{valueOfIntegral}";

            return str;
        }

        [DllImport("\\..\\..\\..\\..\\x64\\Debug\\Dll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Interpolation(int nx, int ny, double[] x, double[] y,
            double[] bc, double[] scoeff, int nsite, double[] site,
            int ndorder, int[] dorder, double[] interpolationValues,
            int nlim, double[] llim, double[] rlim, double[] integrationValues, ref int ret);

        public IEnumerator<string> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
