using ConsoleApp1;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace ViewModelTests
{
    public static class FunctionNames
    {
        public static string cube = "cube", linear = "linear", random = "random";

        public static List<string> ToList()
        {
            return new List<string> { cube, linear, random };
        }
    }

    public class UIFunctionsTest : IUIFunctions
    {

        public string UIFromFile()
        {
            return "test.txt";
        }

        public string UISave()
        {
            return "test.txt";
        }
    }
    public class ViewDataTests
    {
        [Fact]
        public void LoadNullTest()
        {
            var viewData = new ViewData(2, 6, 5, true, FunctionNames.cube, 100, 12, 36, null, null);
            var exeption = Assert.Throws<Exception>(() => viewData.Load(null));
            Assert.Equal("ERROR: NULL INSTEAD OF FILE NAME", exeption.Message);
        }

        [Fact]
        public void LoadEmptyTest()
        {
            var viewData = new ViewData(2, 6, 5, true, FunctionNames.cube, 100, 12, 36, null, null);
            var exeption = Assert.Throws<Exception>(() => viewData.Load(""));
            Assert.Equal("ERROR: FILE NAME ERROR", exeption.Message);
        }

        [Fact]
        public void SaveNullTest()
        {
            var viewData = new ViewData(2, 6, 5, true, FunctionNames.cube, 100, 12, 36, null, null);
            var exeption = Assert.Throws<Exception>(() => viewData.Save(null));
            Assert.Equal("ERROR: NULL INSTEAD OF FILE NAME", exeption.Message);
        }

        [Fact]
        public void SaveEmptyTest()
        {
            var viewData = new ViewData(2, 6, 5, true, FunctionNames.cube   , 100, 12, 36, null, null);
            var exeption = Assert.Throws<Exception>(() => viewData.Save(""));
            Assert.Equal("ERROR: FILE NAME ERROR", exeption.Message);
        }

        [Fact]
        public void NumberOfNodesInvalidTest()
        {
            var viewData = new ViewData(2, 6, 5, true, FunctionNames.cube, 100, 12, 36, null, null);
            viewData.NumberOfNodes = 0;
            Assert.True(viewData[nameof(viewData.NumberOfNodes)] == "Invalid number of nodes");
        }

        [Fact]
        public void ValidationErrorTest()
        {
            var viewData = new ViewData(1, 0, 5, true, FunctionNames.cube, 100, 12, 36, null, null);
            Assert.True(viewData.IsValidationError());
            Assert.True(viewData.IsValidationErrorInRawData());
        }

        [Fact]
        public void FromControlsCommandTest()
        {
            var uiFunctionTest = new UIFunctionsTest();
            var viewData = new ViewData(2, 6, 5, true, FunctionNames.cube, 9, 12, 36, uiFunctionTest, null);
            viewData.FromControlsCommand.Execute(null);
            //Assert.True(viewData.rawDataList.Contains("Coordinate: " + "2.000" + "\nValue: " + "8.000"));
            Assert.Equal(5, viewData.RawDataList.Count);
            Assert.Contains("Coordinate: " + "2,000" + "\nValue: " + "8,000", viewData.RawDataList);
            Assert.Contains("Coordinate: " + "3,000" + "\nValue: " + "27,000", viewData.RawDataList);
            Assert.Contains("Coordinate: " + "4,000" + "\nValue: " + "64,000", viewData.RawDataList);
            Assert.Contains("Coordinate: " + "5,000" + "\nValue: " + "125,000", viewData.RawDataList);
            Assert.Contains("Coordinate: " + "6,000" + "\nValue: " + "216,000", viewData.RawDataList);

            var templist = new List<SplineDataItem>();
            //templist = viewData.SplineDataList.Select(x => Math.Round(x.value, 5));
            foreach(var item in viewData.SplineDataListEnumerable)
            {
                templist.Add(new SplineDataItem(Math.Round(item.Coordinate, 5),
                    Math.Round(item.Value, 5),
                    Math.Round(item.ValueOfFirstDerivative, 5),
                    Math.Round(item.ValueOfSecondDerivative, 5)));
            }

            Assert.Equal(9, viewData.SplineDataList.Count);

            var splineDataItem = new SplineDataItem(2, 8, 12, 12);
            Assert.Contains(splineDataItem, templist);

            splineDataItem = new SplineDataItem(2.5, 15.625, 18.75, 15);
            Assert.Contains(splineDataItem, templist);
            
            splineDataItem = new SplineDataItem(6, 216, 108, 36);
            Assert.Contains(splineDataItem, templist);
        }

        [Fact]
        public void SaveFromFileCommandsTest()
        {
            var uiFunctionTest = new UIFunctionsTest();
            var viewData = new ViewData(2, 6, 5, true, FunctionNames.cube, 9, 12, 36, uiFunctionTest, null);
            viewData.SaveCommand.Execute(null);
            viewData.LeftLimitOfSegment = 0;
            viewData.RightLimitOfSegment = 1;
            viewData.NumberOfNodes = 10;
            viewData.IsUniform = false;
            viewData.Function = FunctionNames.linear;
            viewData.FromFileCommand.Execute(null);
            viewData.LeftLimitOfSegment.Should().Be(2);
            viewData.RightLimitOfSegment.Should().Be(6);
            viewData.NumberOfNodes.Should().Be(5);
            viewData.IsUniform.Should().Be(true);
            Assert.Equal(FunctionNames.cube, viewData.Function);
        }
    }
}
