using ConsoleApp1;
//using ConsoleApp21;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WPF
{
    
    public class ViewData: INotifyPropertyChanged, IDataErrorInfo
    {
        public RawData rawData;

        public SplineData splineData;

        public event PropertyChangedEventHandler PropertyChanged;
        //public event Function1 InvalidInput;

        private double leftLimitOfSegment;
        private double rightLimitOfSegment;
        private int numberOfNodes;
        private bool isUniform;
        private FRaw function;

        public double LeftLimitOfSegment
        {
            get { return leftLimitOfSegment; }
            set
            { 
                leftLimitOfSegment = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("leftLimitOfSegment"));

            }
        }
        
        public double RightLimitOfSegment
        {
            get { return rightLimitOfSegment; }
            set 
            {
                rightLimitOfSegment= value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("rightLimitOfSegment"));
            }
        }
        public int NumberOfNodes 
        { 
            get { return numberOfNodes; }
            set
            {
                numberOfNodes = value;
                if (PropertyChanged != null) 
                    PropertyChanged(this, new PropertyChangedEventArgs("numberOfNodes"));
            }
        }
        public bool IsUniform 
        {
            get { return isUniform; }
            set
            {
                isUniform = value;
                if (PropertyChanged != null) 
                    PropertyChanged(this, new PropertyChangedEventArgs("isUniform"));
            }
        }
        public FRaw Function 
        { 
            get { return function; }
            set
            {
                function = value;
                if (PropertyChanged != null) 
                    PropertyChanged(this, new PropertyChangedEventArgs("function"));
            }
        }

        public int numberOfNodesToCalculateValues { get; set; }

        public double valueOfSecondDerivativeInTheRightLimit { get; set; }
        public double valueOfSecondDerivativeInTheLeftLimit { get; set; }
        
        // public double[] newNodesOfgrid { get; set; }

        public string Error
        {
            get;
            set;
        }

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                //if ((columnName == "RightLimitOfSegment") || (columnName == "LeftLimitOfSegment"))
                //{
                //    if (LeftLimitOfSegment >= RightLimitOfSegment)
                //        return "Values cannot be the same";
                //}
                switch (columnName)
                {
                    case nameof(RightLimitOfSegment):
                        if (RightLimitOfSegment <= LeftLimitOfSegment)
                        {
                            error = "Invalid right limit";
                        }
                        break;
                    case nameof(LeftLimitOfSegment):
                        if (LeftLimitOfSegment >= RightLimitOfSegment)
                        {
                            error = "Invalid left limit";
                        }
                        break;
                    case nameof(NumberOfNodes):
                        if (NumberOfNodes < 2)
                        {
                            error = "Invalid number of nodes";
                        }
                        break;
                    case nameof(numberOfNodesToCalculateValues):
                        if(numberOfNodesToCalculateValues < 2) //|| numberOfNodesToCalculateValues != newNodesOfgrid.Length)
                        {
                            error = "Invalid number of nodes to calculate values";
                        }
                        break;
                    //case nameof(newNodesOfgrid):
                    //    if(newNodesOfgrid.Length != numberOfNodesToCalculateValues)
                    //    {
                    //        error = "Invalid nodes to calculate values";
                    //    }
                    //    //error = this[nameof(numberOfNodesToCalculateValues)];
                    //    break;
                    case nameof(valueOfSecondDerivativeInTheLeftLimit):
                        break;
                    case nameof(valueOfSecondDerivativeInTheRightLimit):
                        break;
                }
                return error;
            }
        }

        public ViewData(double leftLimitOfSegment, double rightLimitOfSegment, 
            int numberOfNodes, bool isUniform, FRaw function, 
            int numberOfNodesToCalculateValues, 
            double valueOfSecondDerivativeInTheLeftLimit, 
            double valueOfSecondDerivativeInTheRightLimit)// double[] newNodesOfgrid)
        {
            this.LeftLimitOfSegment = leftLimitOfSegment;
            this.RightLimitOfSegment = rightLimitOfSegment;
            this.NumberOfNodes = numberOfNodes;
            this.IsUniform = isUniform;
            this.Function = function;
            this.numberOfNodesToCalculateValues = numberOfNodesToCalculateValues;
            this.valueOfSecondDerivativeInTheRightLimit = valueOfSecondDerivativeInTheRightLimit;
            this.valueOfSecondDerivativeInTheLeftLimit = valueOfSecondDerivativeInTheLeftLimit;
            //this.newNodesOfgrid = newNodesOfgrid;

            this.rawData = new RawData(leftLimitOfSegment, rightLimitOfSegment, numberOfNodes, isUniform, function);
            this.splineData = new SplineData(rawData,
                valueOfSecondDerivativeInTheLeftLimit,
                valueOfSecondDerivativeInTheRightLimit,
                numberOfNodesToCalculateValues);
                 //newNodesOfgrid);
        }

        public void Save(string filename)
        {
            RawData obj = new RawData(leftLimitOfSegment, rightLimitOfSegment, numberOfNodes, isUniform, function);
            obj.initializeTheValues();
            obj.Save(filename);
        }

        public void Load(string filename)
        {
            RawData obj = new RawData(filename);
            LeftLimitOfSegment = obj.leftLimitOfSegment;
            RightLimitOfSegment = obj.rightLimitOfSegment;
            NumberOfNodes = obj.numberOfNodes;
            IsUniform = obj.isUniform;
            Function = obj.function;
            //rawData = obj;
            
        }

        public void inputFromControls()
        {
            var rawdata = new RawData(leftLimitOfSegment, RightLimitOfSegment, numberOfNodes, isUniform, function);
            rawData = rawdata;
            rawData.initializeTheValues();

            var splinedata = new SplineData(rawData,
                valueOfSecondDerivativeInTheLeftLimit,
                valueOfSecondDerivativeInTheRightLimit,
                numberOfNodesToCalculateValues);
                //newNodesOfgrid);
            splineData = splinedata;
            splineData.splineConstruction();
        }

        public delegate void Function1(string a);
    }
}
