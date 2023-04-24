using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace View
{
    public static class FunctionNames
    {
        //public static string[] names = new string[3] { "cube", "linear", "random" };  //cube = "cube", linear = "linear", random = "random";
        public static string cube = "cube", linear = "linear", random = "random";

        public static List<string> ToList()
        {
            return new List<string> {cube, linear, random };
        }
    }
}
