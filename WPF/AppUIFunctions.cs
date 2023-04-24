using OxyPlot.Legends;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using Microsoft.Win32;

namespace WPF
{
    public class AppUIFunctions : IUIFunctions
    {

        private PlotModel plotModel;
        public AppUIFunctions() //PlotModel plotModel
        {
            //this.plotModel = plotModel;
        }

        public string UISave()
        {
            string fileName = string.Empty;
            SaveFileDialog newFile = new SaveFileDialog()
            {
                Title = "Save",
                Filter = "Text Document (*.txt) | *.txt",
                FileName = ""
            };
            if (newFile.ShowDialog() == true)
            {
                fileName = newFile.FileName;
                return fileName;
            }
            return null;
        }

        public string UIFromFile()
        {
            string fileName = string.Empty;
            OpenFileDialog file = new OpenFileDialog()
            {
                Title = "Load",
                Filter = "Text Document (*.txt) | *.txt",
                FileName = ""
            };
            if (file.ShowDialog() == true)
            {
                fileName = file.FileName;
                return fileName;
            }
            return null;
        }

        //public void DrawSpline(double[] rawDataX, double[] rawDataY, double[] splineDataX, double[] splineDataY)
        //{
        //    var legend = new Legend();
        //    plotModel.Legends.Add(legend);
        //    plotModel.Title = "Spline and discrete data";
        //    plotModel.Series.Clear();

        //    var splineLineseries = new LineSeries();
        //    OxyColor color = OxyColors.Blue;
        //    for (int i = 0; i < splineDataX.Length; i++)
        //    {
        //        splineLineseries.Points.Add(new DataPoint(splineDataX[i], splineDataY[i]));
        //    }
        //    splineLineseries.Color = color;
        //    splineLineseries.MarkerStroke = color;
        //    splineLineseries.MarkerFill = color;
        //    splineLineseries.MarkerSize = 4;
        //    splineLineseries.MarkerType = MarkerType.None;
        //    splineLineseries.Title = "Spline data";
        //    plotModel.Series.Add(splineLineseries);

        //    var discreteLineseries = new LineSeries();
        //    color = OxyColors.Orange;
        //    for (int i = 0; i < rawDataX.Length; i++)
        //    {
        //        discreteLineseries.Points.Add(new DataPoint(rawDataX[i], rawDataY[i]));
        //    }
        //    discreteLineseries.Color = color;
        //    discreteLineseries.MarkerStroke = color;
        //    discreteLineseries.MarkerFill = color;
        //    discreteLineseries.MarkerSize = 4;
        //    discreteLineseries.MarkerType = MarkerType.Circle;
        //    discreteLineseries.Title = "Discrete data";
        //    plotModel.Series.Add(discreteLineseries);
        //}
    }
}
