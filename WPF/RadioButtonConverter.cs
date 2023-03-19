using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF
{
    [ValueConversion(typeof(int), typeof(bool))]
    public class RadioButtonConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tmp = true;
            if (parameter.ToString() == "true")
                tmp = true;
            else
                tmp = false;
            if (tmp == (bool)value)
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            var tmp = true;
            if (parameter.ToString() == "true")
                tmp = true;
            else
                tmp = false;
            if (tmp == (bool)value)
                return true;
            else
                return false;
        }
    }
}
