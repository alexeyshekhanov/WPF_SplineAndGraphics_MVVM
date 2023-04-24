using System.Windows;
using View;
using ViewModel;

namespace WPF
{
    public partial class MainWindow : Window
    {
        public ViewData viewData;
        
        public MainWindow()
        {
            
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
