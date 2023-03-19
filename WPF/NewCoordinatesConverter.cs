using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF
{
    [ValueConversion(typeof(double[]), typeof(string))]
    public class NewCoordinatesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            var tmp = string.Empty;


            foreach(var a in (double[])value)
            {
                tmp = tmp + System.Convert.ToDouble(a).ToString() + " ";
            }
            
            return tmp;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            string tmp = value as string;
            var y = tmp.Trim().Split(' ');
            var x = new double[y.Length];
            for (int i = 0; i < y.Length; i++)
            {
                x[i] = System.Convert.ToDouble(y[i]);

            }
            return x;
        }

    }

}
