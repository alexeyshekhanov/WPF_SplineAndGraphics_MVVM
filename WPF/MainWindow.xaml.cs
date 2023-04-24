//using ConsoleApp1;
//using ConsoleApp2;
//using ConsoleApp21;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using View;
using ViewModel;
//using ViewModel;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewData viewData;
        
        public MainWindow()
        {
            //var plotModel = new PlotModel();
            
            AppUIFunctions appUIFunctions = new AppUIFunctions();
            MessageNotifier messageNotifier = new MessageNotifier();
            viewData = new ViewData(2, 6, 5, true, FunctionNames.cube, 100, 12, 36, appUIFunctions, messageNotifier);
            InitializeComponent();
            comboBoxCreation();
            this.DataContext = viewData;
        }

        private void comboBoxCreation()
        {
            foreach (var value in FunctionNames.ToList())   //Enum.GetValues(typeof(FRawEnum))
            {
                ComboBoxRawDataListOfFunctions.Items.Add(value);
            }
        }
    }
}
