using ConsoleApp1;
using FluentAssertions;

namespace ClassLibraryTests
{
    public class RawDataTests
    {
        [Fact]
        public void initializeTheValuesCubeTest()
        {
            var rawData = new RawData(0, 10, 3, true, Functions.cube);
            rawData.initializeTheValues();
            var nodes = new double[3] { 0, 5, 10 };
            var valuesInNodes = new double[3] {0, 125, 1000 };
            Assert.Equal(nodes, rawData.nodesOfGrid);
            Assert.Equal(valuesInNodes, rawData.valuesInNodes);
        }

        [Fact]
        public void initializeTheValuesRandomTest()
        {
            var rawData = new RawData(0, 1, 7, false, Functions.random);
            rawData.nodesOfGrid.Should().BeInAscendingOrder();
            Assert.True(rawData.valuesInNodes.All(x => 0 < x && x < 1));
        }

        [Fact]
        public void LoadNullTest()
        {
            var rawData = new RawData();
            var exeption = Assert.Throws<Exception>(() => RawData.Load(null, ref rawData));
            Assert.Equal("ERROR: NULL INSTEAD OF FILE NAME", exeption.Message);
        }

        [Fact]
        public void LoadEmptyTest() 
        {
            var rawData = new RawData();
            var exeption = Assert.Throws<Exception>(() => RawData.Load("", ref rawData));
            Assert.Equal("ERROR: FILE NAME ERROR", exeption.Message);
        }

        [Fact]
        public void ConstructorLoadNullTest()
        {
            var exeption = Assert.Throws<Exception>(() => new RawData(null));
            Assert.Equal("ERROR: NULL INSTEAD OF FILE NAME", exeption.Message);
        }

        [Fact]
        public void ConstructorLoadEmptyTest()
        {
            var rawData = new RawData();
            var exeption = Assert.Throws<Exception>(() => new RawData(""));
            Assert.Equal("ERROR: FILE NAME ERROR", exeption.Message);
        }

        [Fact]
        public void SaveNullTest()
        {
            var rawData = new RawData();
            var exeption = Assert.Throws<Exception>(() => rawData.Save(null));
            Assert.Equal("ERROR: NULL INSTEAD OF FILE NAME", exeption.Message);
        }

        [Fact]
        public void SaveEmptyTest()
        {
            var rawData = new RawData();
            var exeption = Assert.Throws<Exception>(() => rawData.Save(""));
            Assert.Equal("ERROR: FILE NAME ERROR", exeption.Message);
        }

    }
}