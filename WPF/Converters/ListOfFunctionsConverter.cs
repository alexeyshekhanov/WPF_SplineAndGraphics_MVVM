//using ConsoleApp2;
using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF
{
    [ValueConversion(typeof(FRaw), typeof(string))]
    public class ListOfFunctionsConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((FRaw)value == Functions.Cube)
            {
                return "cube";
            }
            else if ((FRaw)value == Functions.Linear)
            {
                return "linear";
            }
            else if ((FRaw)value == Functions.MyRandom)
            {
                return "random";
            }
            return "err";
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            FRaw tmp = Functions.Cube;
            if (value.ToString() == "cube")
            {
                tmp = Functions.Cube;
                return tmp;
            }
            else if (value.ToString() == "linear")
            {
                tmp = Functions.Linear;
                return tmp;
            }
            else if (value.ToString() == "random")
            {
                tmp = Functions.MyRandom;
                return tmp;
            }
            return tmp;
        }
    }
}
