using ConsoleApp1;
//using ConsoleApp2;
//using ConsoleApp21;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewData viewData = new ViewData(2, 6, 5, true, Functions.cube, 100, 12, 36);
        
        public MainWindow()
        {
            InitializeComponent();
            //RawData a = new RawData(2, 10, 5, true, cube);
            //a.initializeTheValues();
            //a.Save("test11.txt");
            //RawData b = new RawData("test11.txt");
            //TextBlockValueOfTheIntegral.Text = b.ToLongString("{0:0.000}");
            comboBoxCreation();
            //viewData.InvalidInput += ShowErrorMessageDialog;
            this.DataContext = viewData;
            ListBoxOutputSplineData.DataContext = viewData.splineData.calculatedSplineValues;
        }

        private void comboBoxCreation()
        {
            foreach (var value in Enum.GetValues(typeof(FRawEnum)))
            {
                ComboBoxRawDataListOfFunctions.Items.Add(value);
            }
        }
        
        private void drawSpline()
        {
            //splinePlot.Plot.Clear();
            //var dataSplineX = new double[viewData.numberOfNodesToCalculateValues];
            //var dataSplineY = new double[viewData.numberOfNodesToCalculateValues];
            //for(int i = 0; i < viewData.numberOfNodesToCalculateValues; i++)
            //{
            //    dataSplineX[i] = viewData.splineData.calculatedSplineValues[i].coordinate;
            //    dataSplineY[i] = viewData.splineData.calculatedSplineValues[i].value;
            //}
            //splinePlot.Plot.AddScatterLines(dataSplineX, dataSplineY);
            //splinePlot.Refresh();
            //var dataDiscreteX = new double[viewData.NumberOfNodes];
            //var dataDiscreteY = new double[viewData.NumberOfNodes];
            //for (int i = 0; i < viewData.NumberOfNodes; i++)
            //{
            //    dataDiscreteX[i] = viewData.rawData.nodesOfGrid[i];
            //    dataDiscreteY[i] = viewData.rawData.valuesInNodes[i];
            //}
            //splinePlot.Plot.AddScatter(dataDiscreteX, dataDiscreteY);
            //splinePlot.Refresh();

            var plotModel = new PlotModel();
            var legend = new Legend();
            plotModel.Legends.Add(legend);
            plotModel.Title = "Spline and discrete data";
            plotModel.Series.Clear();
            

            var splineLineseries = new LineSeries();
            OxyColor color = OxyColors.Blue;
            for (int i = 0; i < viewData.numberOfNodesToCalculateValues; i++)
            {
                splineLineseries.Points.Add(new DataPoint(
                    viewData.splineData.calculatedSplineValues[i].coordinate, 
                    viewData.splineData.calculatedSplineValues[i].value));
            }
            splineLineseries.Color = color;
            splineLineseries.MarkerStroke = color;
            splineLineseries.MarkerFill = color;
            splineLineseries.MarkerSize = 4;
            splineLineseries.MarkerType = MarkerType.None;
            splineLineseries.Title = "Spline data";
            plotModel.Series.Add(splineLineseries);

            var discreteLineseries = new LineSeries();
            color = OxyColors.Orange;
            for (int i = 0; i < viewData.NumberOfNodes; i++)
            {
                discreteLineseries.Points.Add(new DataPoint(
                    viewData.rawData.nodesOfGrid[i],
                    viewData.rawData.valuesInNodes[i]));
            }
            discreteLineseries.Color = color;
            discreteLineseries.MarkerStroke = color;
            discreteLineseries.MarkerFill = color;
            discreteLineseries.MarkerSize = 4;
            discreteLineseries.MarkerType = MarkerType.Circle;
            discreteLineseries.Title = "Discrete data";
            plotModel.Series.Add(discreteLineseries);

            oxyPlot.Model = plotModel;
        }

        private void outputAndGraphics(object sender, ExecutedRoutedEventArgs e)
        {
            onClickFromControls(sender, e);
            drawSpline();
        }

        private void dataValidationFromControls(object sender, CanExecuteRoutedEventArgs e)
        {
            if (isValidationError())
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
        }

        private void dataValidationFromFile(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                fromFileToControls();
                //viewData.inputFromControls();
                if (isValidationError())
                {
                    e.CanExecute = false;
                }
                else
                {
                    e.CanExecute = true;
                }
            }
            catch(Exception ex)
            {
                ShowErrorMessageDialog(ex.Message);
                return;
            }
        }

        private void onClickFromFileCommand(object sender, RoutedEventArgs e)
        {
            CustomCommands.FromFile.Execute("", null);
        }

        private void saveToFile(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                onClickSave(sender, e);
            }
            catch(Exception ex)
            {
                ShowErrorMessageDialog(ex.Message);
                return;
            }
        }

        private void dataValidationForSave(object sender, CanExecuteRoutedEventArgs e)
        {
            if (isValidationErrorInRawData())
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
        }

        private void onClickSave(object sender, RoutedEventArgs e)
        {
            try
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
                }
                viewData.Save(fileName);
            }
            catch (Exception ex) 
            {
                ShowErrorMessageDialog(ex.Message);
                return;
            }           
        }

        private void fromFileToControls()
        {
            try
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
                    viewData.Load(fileName);
                }
                //viewData.Load(fileName);
            }
            catch (Exception ex)
            {
                ShowErrorMessageDialog(ex.Message);
                return;
            }
        }

        private void onClickFromFile(object sender, RoutedEventArgs e)
        {
            try
            {
                fromFileToControls();
                viewData.inputFromControls();
                outputData();
            }
            catch (Exception ex)
            {
                ShowErrorMessageDialog(ex.Message);
                return;
            }
        }

        private void onClickFromControls(object sender, RoutedEventArgs e)
        {
            try
            {
                viewData.inputFromControls();

                outputData();
            }
            catch(Exception ex)
            {
                ShowErrorMessageDialog(ex.Message);
                return;
            }
        }

        private void outputData()
        {
            ListBoxOutputRawData.ItemsSource = viewData.rawData;
            ListBoxOutputSplineData.ItemsSource = viewData.splineData.calculatedSplineValues;
            TextBlockValueOfTheIntegral.Text = viewData.splineData.valueOfIntegral.ToString();
        }

        private void ShowErrorMessageDialog(string error)
        {
            MessageBox.Show(error, "MY_ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        private void ShowWarningMessageDialog(string error)
        {
            MessageBox.Show(error, "MY_WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        private bool isValidationErrorInRawData()
        {
            if (Validation.GetHasError(TextBoxRawDataNumberOfNodes) ||
                Validation.GetHasError(TextBoxRawDataRightLimit) ||
                Validation.GetHasError(TextBoxRawDataLeftLimit))
                return true;
            return false;
        }

        private bool isValidationError()
        {
            if (Validation.GetHasError(TextBoxRawDataNumberOfNodes) ||
                Validation.GetHasError(TextBoxRawDataRightLimit) ||
                Validation.GetHasError(TextBoxRawDataLeftLimit) ||
                Validation.GetHasError(TextBoxSplineDataNumberOfNodes))
                return true;
            return false;
        }
        private void ValidationErrorHandler(object sender, ValidationErrorEventArgs e)
        {
            ShowErrorMessageDialog(e.Error.ErrorContent.ToString());
        }

    }
}
