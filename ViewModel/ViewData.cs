using ConsoleApp1;
using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;

namespace ViewModel
{
    
    public class ViewData: INotifyPropertyChanged, IDataErrorInfo
    {
        public RawData rawData;

        public SplineData splineData;

        public event PropertyChangedEventHandler PropertyChanged;
        //public event Function1 InvalidInput;

        private void onPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        private double leftLimitOfSegment;
        private double rightLimitOfSegment;
        private int numberOfNodes;
        private bool isUniform;
        private string function;
        private double? valueOfIntegral;

        public double[] nodesOfGridRD;
        public double[] valuesInNodesRD;

        public ObservableCollection<string> rawDataList { get; set; }
        public ObservableCollection<SplineDataItem> splineDataList { get; set; }

        public IEnumerable<SplineDataItem> SplineDataList => splineDataList;

        private IUIFunctions uiFunctions;

        private IExceptionNotifier exceptionNotifier;

        private readonly ICommand fromControlsCommand;
        private readonly ICommand saveCommand;
        private readonly ICommand fromFileCommand;

        public ICommand FromControlsCommand { get => fromControlsCommand; }
        public ICommand SaveCommand { get => saveCommand; }
        public ICommand FromFileCommand { get => fromFileCommand; }

        public PlotModel plotModel { get; set; }

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
        public string Function 
        { 
            get { return function; }
            set
            {
                function = value;
                if (PropertyChanged != null) 
                    PropertyChanged(this, new PropertyChangedEventArgs("function"));
            }
        }

        public double? ValueOfIntegral
        {
            get { return valueOfIntegral; }
            set
            {
                valueOfIntegral= value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("valueOfIntegral"));
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
            int numberOfNodes, bool isUniform, string function, 
            int numberOfNodesToCalculateValues, 
            double valueOfSecondDerivativeInTheLeftLimit, 
            double valueOfSecondDerivativeInTheRightLimit, 
            IUIFunctions uiFunctions,
            IExceptionNotifier exceptionNotifier)// double[] newNodesOfgrid)
        {
            this.LeftLimitOfSegment = leftLimitOfSegment;
            this.RightLimitOfSegment = rightLimitOfSegment;
            this.NumberOfNodes = numberOfNodes;
            this.IsUniform = isUniform;
            this.Function = function;
            this.nodesOfGridRD = new double[numberOfNodes];
            this.valuesInNodesRD = new double[numberOfNodes];
            this.numberOfNodesToCalculateValues = numberOfNodesToCalculateValues;
            this.valueOfSecondDerivativeInTheRightLimit = valueOfSecondDerivativeInTheRightLimit;
            this.valueOfSecondDerivativeInTheLeftLimit = valueOfSecondDerivativeInTheLeftLimit;
            //this.newNodesOfgrid = newNodesOfgrid;


            this.rawData = new RawData(leftLimitOfSegment, rightLimitOfSegment, numberOfNodes, isUniform, StringToFrawConverter(function));
            this.splineData = new SplineData(rawData,
                valueOfSecondDerivativeInTheLeftLimit,
                valueOfSecondDerivativeInTheRightLimit,
                numberOfNodesToCalculateValues);
            //newNodesOfgrid);

            rawDataList = new ObservableCollection<string>();

            splineDataList = new ObservableCollection<SplineDataItem>();
            
            this.uiFunctions = uiFunctions;
            this.exceptionNotifier = exceptionNotifier;

            saveCommand = new RelayCommand(
                _ => !isValidationErrorInRawData(),
                _ =>
                {
                    try
                    {
                        Save(uiFunctions.UISave());
                    }
                    catch (Exception ex)
                    {
                        exceptionNotifier.ShowErrorMessage(ex.Message);
                        return;
                    }                    
                });

            fromControlsCommand = new RelayCommand(
                _ => !isValidationError(),
                _ => 
                {
                    try
                    {
                        //outputAndGraphics();
                        inputFromControls();
                        outputData();
                        drawSpline();
                    }
                    catch (Exception ex)
                    {
                        exceptionNotifier.ShowErrorMessage(ex.Message);
                        return;
                    }
                    
                }
                );
            fromFileCommand = new RelayCommand(
                _ => true,
                _ =>
                {
                    try
                    {
                        Load(uiFunctions.UIFromFile());
                        if (!isValidationError())
                        {
                            //outputAndGraphicsFromFile();
                            inputFromFile();
                            outputData();
                            drawSpline();
                        }
                        else
                        {
                            exceptionNotifier.ShowWarningMessage("File has been loaded with invalid values");
                        }
                    }
                    catch(Exception ex)
                    {
                        exceptionNotifier.ShowErrorMessage(ex.Message);
                        return;
                    }

                });
        }

        //public void outputAndGraphicsFromFile()
        //{
        //    try
        //    {
        //        inputFromFile();
        //        outputData();
        //        drawSpline();
        //    }
        //    catch (Exception ex)
        //    {
        //        exceptionNotifier.ShowErrorMessage(ex.Message);
        //        return;
        //    }

        //}

        //private void outputAndGraphics()
        //{
        //    try
        //    {
        //        inputFromControls();
        //        outputData();
        //        drawSpline();
        //    }
        //    catch(Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //private void outputAndGraphics()
        //{
        //    try
        //    {
        //        inputFromControls();
        //        outputData();

        //        var splineDataX = new double[numberOfNodesToCalculateValues];
        //        var splineDataY = new double[numberOfNodesToCalculateValues];
        //        for (int i = 0; i < numberOfNodesToCalculateValues; i++)
        //        {
        //            splineDataX[i] = splineData.calculatedSplineValues[i].coordinate;
        //            splineDataY[i] = splineData.calculatedSplineValues[i].value;
        //        }
        //        var rawDataX = new double[NumberOfNodes];
        //        var rawDataY = new double[NumberOfNodes];
        //        rawDataX = rawData.nodesOfGrid;
        //        rawDataY = rawData.valuesInNodes;
        //        uiFunctions.DrawSpline(rawDataX, rawDataY, splineDataX, splineDataY);
                
        //    }
        //    catch (Exception ex)
        //    {
        //        exceptionNotifier.ShowErrorMessage(ex.Message);
        //        return;
        //    }
        //}

        public void outputData()
        {
            rawDataList.Clear();
            splineDataList.Clear();
            for (int i = 0; i < NumberOfNodes; i++)
            {
                rawDataList.Add("Coordinate: " + string.Format("{0:0.000}", rawData.nodesOfGrid[i]) + 
                    "\nValue: " + string.Format("{0:0.000}", rawData.valuesInNodes[i]));
            }
            for (int i = 0; i < numberOfNodesToCalculateValues; i++)
            {
                var temp = new SplineDataItem(
                    splineData.calculatedSplineValues[i].coordinate,
                    splineData.calculatedSplineValues[i].value,
                    splineData.calculatedSplineValues[i].valueOfFirstDerivative,
                    splineData.calculatedSplineValues[i].valueOfSecondDerivative
                    );
                splineDataList.Add(temp);
            }
            ValueOfIntegral = splineData.valueOfIntegral;
        }

        public bool isValidationErrorInRawData()
        {
            if (this[nameof(LeftLimitOfSegment)] != "Invalid left limit" &&
                this[nameof(RightLimitOfSegment)] != "Invalid right limit" &&
                this[nameof(NumberOfNodes)] != "Invalid number of nodes")
                return false;
            return true;
        }

        public bool isValidationError()
        {
            if (!isValidationErrorInRawData() &&
                this[nameof(numberOfNodesToCalculateValues)] != "Invalid number of nodes to calculate values")
                return false;
            return true;
        }

        public void Save(string filename)
        {
            try
            {
                RawData obj = new RawData(leftLimitOfSegment, rightLimitOfSegment, numberOfNodes, isUniform, StringToFrawConverter(function), rawData.nodesOfGrid, rawData.valuesInNodes);
                obj.Save(filename);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }  
        }

        public void Load(string filename)
        {
            try
            {
                RawData obj = new RawData(filename);
                LeftLimitOfSegment = obj.leftLimitOfSegment;
                RightLimitOfSegment = obj.rightLimitOfSegment;
                NumberOfNodes = obj.numberOfNodes;
                IsUniform = obj.isUniform;
                Function = FrawToStringConverter(obj.function);
                nodesOfGridRD = obj.nodesOfGrid;
                valuesInNodesRD = obj.valuesInNodes;
                //rawData = obj;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }  
        }

        public void inputFromFile()
        {
            var rawdata = new RawData(leftLimitOfSegment, RightLimitOfSegment, numberOfNodes, isUniform, StringToFrawConverter(function), nodesOfGridRD, valuesInNodesRD);
            rawData = rawdata;

            var splinedata = new SplineData(rawData,
                valueOfSecondDerivativeInTheLeftLimit,
                valueOfSecondDerivativeInTheRightLimit,
                numberOfNodesToCalculateValues);
            //newNodesOfgrid);
            splineData = splinedata;
            splineData.splineConstruction();
        }

        public void inputFromControls()
        {
            var rawdata = new RawData(leftLimitOfSegment, RightLimitOfSegment, numberOfNodes, isUniform, StringToFrawConverter(function));
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

        public void drawSpline()
        {
            this.plotModel = new PlotModel();
            var legend = new Legend();
            plotModel.Legends.Add(legend);
            plotModel.Title = "Spline and discrete data";
            plotModel.Series.Clear();

            var splineLineseries = new LineSeries();
            OxyColor color = OxyColors.Blue;
            for (int i = 0; i < numberOfNodesToCalculateValues; i++)
            {
                splineLineseries.Points.Add(new DataPoint(
                    splineData.calculatedSplineValues[i].coordinate,
                    splineData.calculatedSplineValues[i].value));
            }
            splineLineseries.Color = color;
            splineLineseries.MarkerStroke = color;
            splineLineseries.MarkerFill = color;
            splineLineseries.MarkerSize = 4;
            splineLineseries.MarkerType = MarkerType.None;
            splineLineseries.Title = "Spline data";
            plotModel.Series.Add(splineLineseries);

            var discreteLineseries = new LineSeries();
            color = OxyColors.Orange;
            for (int i = 0; i < NumberOfNodes; i++)
            {
                discreteLineseries.Points.Add(new DataPoint(
                    rawData.nodesOfGrid[i],
                    rawData.valuesInNodes[i]));
            }
            discreteLineseries.Color = color;
            discreteLineseries.MarkerStroke = color;
            discreteLineseries.MarkerFill = color;
            discreteLineseries.MarkerSize = 4;
            discreteLineseries.MarkerType = MarkerType.Circle;
            discreteLineseries.Title = "Discrete data";
            plotModel.Series.Add(discreteLineseries);
            onPropertyChanged(nameof(plotModel));
        }

        public FRaw StringToFrawConverter(string func)
        {
            FRaw tmp = Functions.cube;
            if (func == "cube")
            {
                tmp = Functions.cube;
                return tmp;
            }
            else if (func == "linear")
            {
                tmp = Functions.linear;
                return tmp;
            }
            else if (func == "random")
            {
                tmp = Functions.random;
                return tmp;
            }
            return tmp;
        }

        public string FrawToStringConverter(FRaw fRaw)
        {
            if (fRaw == Functions.cube)
            {
                return "cube";
            }
            else if (fRaw == Functions.linear)
            {
                return "linear";
            }
            else if (fRaw == Functions.random)
            {
                return "random";
            }
            return "err";
        }

        public delegate void Function1(string a);
    }
}
