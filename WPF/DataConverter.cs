using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WPF
{
    [ValueConversion(typeof(double[]), typeof(string))]
    public class DataConverter: IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var tmp = value[0].ToString() + " " + value[1].ToString();
            return tmp;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
                string tmp = value as string;
                var y = tmp.Trim().Split(' ');

                return new object[] { System.Convert.ToDouble(y[0]), System.Convert.ToDouble(y[1]) };
        }
        
    }
}
