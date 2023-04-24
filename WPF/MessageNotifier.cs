using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModel;

namespace WPF
{
    public class MessageNotifier : IExceptionNotifier
    {
        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "MY_ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "MY_WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
