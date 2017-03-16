using Microsoft.VisualStudio.TestTools.UnitTesting;
using MosaicGenerator.Abstractions;
using MosaicGenerator.Tests.Dummy;
using System.Threading.Tasks;
using Windows.UI;
using MosaicGenerator.Implementations;

namespace MosaicGenerator.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestAverageCalculator()
        {
            var white = Color.FromArgb(0, 255, 255, 255);
            var reader = new DummyPixelReader(white);
            IAverageColorCalculator calculator = new AverageColorCalculator(reader);

            var average = await calculator.CalculateAverage(null);

            Assert.AreEqual(white, average);
        }
    }
}