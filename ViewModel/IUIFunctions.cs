using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public interface IUIFunctions
    {
        string UISave();

        string UIFromFile();

        //void DrawSpline(double[] rawDataX, double[] rawDataY, double[] splineDataX, double[] splineDataY);
    }
}
