using ConsoleApp1;
using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ViewModel
{
    
    public class ViewData: INotifyPropertyChanged, IDataErrorInfo
    {
        public RawData rawData;

        public SplineData splineData;

        public event PropertyChangedEventHandler PropertyChanged;
        //public event Function1 InvalidInput;

        private void OnPropertyChanged(string propertyname)
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

        public ObservableCollection<string> RawDataList { get; set; }
        public ObservableCollection<SplineDataItem> SplineDataList { get; set; }

        public IEnumerable<SplineDataItem> SplineDataListEnumerable => SplineDataList;

        private IUIFunctions uiFunctions;

        private IExceptionNotifier exceptionNotifier;

        private readonly ICommand fromControlsCommand;
        private readonly ICommand saveCommand;
        private readonly ICommand fromFileCommand;

        public ICommand FromControlsCommand { get => fromControlsCommand; }
        public ICommand SaveCommand { get => saveCommand; }
        public ICommand FromFileCommand { get => fromFileCommand; }

        public PlotModel PlotModel { get; set; }

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

        public int NumberOfNodesToCalculateValues { get; set; }

        public double ValueOfSecondDerivativeInTheRightLimit { get; set; }
        public double ValueOfSecondDerivativeInTheLeftLimit { get; set; }
        
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
                    case nameof(NumberOfNodesToCalculateValues):
                        if(NumberOfNodesToCalculateValues < 2) 
                        {
                            error = "Invalid number of nodes to calculate values";
                        }
                        break;
                    case nameof(ValueOfSecondDerivativeInTheLeftLimit):
                        break;
                    case nameof(ValueOfSecondDerivativeInTheRightLimit):
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
            IExceptionNotifier exceptionNotifier)
        {
            this.LeftLimitOfSegment = leftLimitOfSegment;
            this.RightLimitOfSegment = rightLimitOfSegment;
            this.NumberOfNodes = numberOfNodes;
            this.IsUniform = isUniform;
            this.Function = function;
            this.nodesOfGridRD = new double[numberOfNodes];
            this.valuesInNodesRD = new double[numberOfNodes];
            this.NumberOfNodesToCalculateValues = numberOfNodesToCalculateValues;
            this.ValueOfSecondDerivativeInTheRightLimit = valueOfSecondDerivativeInTheRightLimit;
            this.ValueOfSecondDerivativeInTheLeftLimit = valueOfSecondDerivativeInTheLeftLimit;


            this.rawData = new RawData(leftLimitOfSegment, rightLimitOfSegment, numberOfNodes, isUniform, StringToFrawConverter(function));
            this.splineData = new SplineData(rawData,
                valueOfSecondDerivativeInTheLeftLimit,
                valueOfSecondDerivativeInTheRightLimit,
                numberOfNodesToCalculateValues);

            RawDataList = new ObservableCollection<string>();

            SplineDataList = new ObservableCollection<SplineDataItem>();
            
            this.uiFunctions = uiFunctions;
            this.exceptionNotifier = exceptionNotifier;

            saveCommand = new RelayCommand(
                _ => !IsValidationErrorInRawData(),
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
                _ => !IsValidationError(),
                _ => 
                {
                    try
                    {
                        //outputAndGraphics();
                        InputFromControls();
                        OutputData();
                        DrawSpline();
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
                        if (!IsValidationError())
                        {
                            //outputAndGraphicsFromFile();
                            InputFromFile();
                            OutputData();
                            DrawSpline();
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
        public void OutputData()
        {
            RawDataList.Clear();
            SplineDataList.Clear();
            for (int i = 0; i < NumberOfNodes; i++)
            {
                RawDataList.Add("Coordinate: " + string.Format("{0:0.000}", rawData.nodesOfGrid[i]) + 
                    "\nValue: " + string.Format("{0:0.000}", rawData.valuesInNodes[i]));
            }
            for (int i = 0; i < NumberOfNodesToCalculateValues; i++)
            {
                var temp = new SplineDataItem(
                    splineData.calculatedSplineValues[i].Coordinate,
                    splineData.calculatedSplineValues[i].Value,
                    splineData.calculatedSplineValues[i].ValueOfFirstDerivative,
                    splineData.calculatedSplineValues[i].ValueOfSecondDerivative
                    );
                SplineDataList.Add(temp);
            }
            ValueOfIntegral = splineData.valueOfIntegral;
        }

        public bool IsValidationErrorInRawData()
        {
            if (this[nameof(LeftLimitOfSegment)] != "Invalid left limit" &&
                this[nameof(RightLimitOfSegment)] != "Invalid right limit" &&
                this[nameof(NumberOfNodes)] != "Invalid number of nodes")
                return false;
            return true;
        }

        public bool IsValidationError()
        {
            if (!IsValidationErrorInRawData() &&
                this[nameof(NumberOfNodesToCalculateValues)] != "Invalid number of nodes to calculate values")
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
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }  
        }

        public void InputFromFile()
        {
            var rawdata = new RawData(leftLimitOfSegment, RightLimitOfSegment, numberOfNodes, isUniform, StringToFrawConverter(function), nodesOfGridRD, valuesInNodesRD);
            rawData = rawdata;

            var splinedata = new SplineData(rawData,
                ValueOfSecondDerivativeInTheLeftLimit,
                ValueOfSecondDerivativeInTheRightLimit,
                NumberOfNodesToCalculateValues);
            splineData = splinedata;
            splineData.SplineConstruction();
        }

        public void InputFromControls()
        {
            var rawdata = new RawData(leftLimitOfSegment, RightLimitOfSegment, numberOfNodes, isUniform, StringToFrawConverter(function));
            rawData = rawdata;
            rawData.InitializeTheValues();

            var splinedata = new SplineData(rawData,
                ValueOfSecondDerivativeInTheLeftLimit,
                ValueOfSecondDerivativeInTheRightLimit,
                NumberOfNodesToCalculateValues);
            splineData = splinedata;
            splineData.SplineConstruction();
        }

        public void DrawSpline()
        {
            this.PlotModel = new PlotModel();
            var legend = new Legend();
            PlotModel.Legends.Add(legend);
            PlotModel.Title = "Spline and discrete data";
            PlotModel.Series.Clear();

            var splineLineseries = new LineSeries();
            OxyColor color = OxyColors.Blue;
            for (int i = 0; i < NumberOfNodesToCalculateValues; i++)
            {
                splineLineseries.Points.Add(new DataPoint(
                    splineData.calculatedSplineValues[i].Coordinate,
                    splineData.calculatedSplineValues[i].Value));
            }
            splineLineseries.Color = color;
            splineLineseries.MarkerStroke = color;
            splineLineseries.MarkerFill = color;
            splineLineseries.MarkerSize = 4;
            splineLineseries.MarkerType = MarkerType.None;
            splineLineseries.Title = "Spline data";
            PlotModel.Series.Add(splineLineseries);

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
            PlotModel.Series.Add(discreteLineseries);
            OnPropertyChanged(nameof(PlotModel));
        }

        public FRaw StringToFrawConverter(string func)
        {
            FRaw tmp = Functions.Cube;
            if (func == "cube")
            {
                tmp = Functions.Cube;
                return tmp;
            }
            else if (func == "linear")
            {
                tmp = Functions.Linear;
                return tmp;
            }
            else if (func == "random")
            {
                tmp = Functions.MyRandom;
                return tmp;
            }
            return tmp;
        }

        public string FrawToStringConverter(FRaw fRaw)
        {
            if (fRaw == Functions.Cube)
            {
                return "cube";
            }
            else if (fRaw == Functions.Linear)
            {
                return "linear";
            }
            else if (fRaw == Functions.MyRandom)
            {
                return "random";
            }
            return "err";
        }

        public delegate void Function1(string a);
    }
}
