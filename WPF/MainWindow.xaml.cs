using ConsoleApp2;
using ConsoleApp21;
using Microsoft.Win32;
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
        public ViewData viewData = new ViewData(2, 10, 5, true, Functions.cube, 5, 12, 60, new double[] { 2.0, 4.0, 6.0, 8.0, 10.0});
        
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

        private void onClickFromFile(object sender, RoutedEventArgs e)
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
                }

                viewData.Load(fileName);
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
                //if (isValidationError())
                  //  throw new Exception("non");
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            viewData.RightLimitOfSegment = 17;
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

        private bool isValidationError()
        {
            if (Validation.GetHasError(TextBoxRawDataNumberOfNodes) ||
                Validation.GetHasError(TextBoxRawDataRightLimit) ||
                Validation.GetHasError(TextBoxRawDataLeftLimit) ||
                Validation.GetHasError(TextBoxSplineDataCoordinates) ||
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
