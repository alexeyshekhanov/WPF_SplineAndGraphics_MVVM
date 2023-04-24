using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public interface IExceptionNotifier
    {
        void ShowErrorMessage(string message);

        void ShowWarningMessage(string message);
    }

}
