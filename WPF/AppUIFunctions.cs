using OxyPlot.Legends;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using Microsoft.Win32;

namespace WPF
{
    public class AppUIFunctions : IUIFunctions
    {

        public string UISave()
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
                return fileName;
            }
            return null;
        }

        public string UIFromFile()
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
                return fileName;
            }
            return null;
        }
    }
}
