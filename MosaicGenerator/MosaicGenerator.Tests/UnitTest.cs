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
        public void TestAverageCalculator()
        {
            var white = Color.FromArgb(0, 255, 255, 255);
            var reader = new DummyPixelReader(white);
            IAverageColorCalculator calculator = new AverageColorCalculator(reader);

            var average = calculator.CalculateAverage(null);
            
            Assert.AreEqual(white, average);
        }

        // Test of de gemiddelde kleur zwart is voor de ingevoerde kleur zwart.
        [TestMethod]
        public void TestAverageCalculatorBlack()
        {
            var black = Color.FromArgb(0, 0, 0, 0);
            var reader = new DummyPixelReader(black);
            IAverageColorCalculator calculator = new AverageColorCalculator(reader);

            var average = calculator.CalculateAverage(null);

            Assert.AreEqual(black, average);
        }

        // Test of het gemiddelde wit wordt teruggegeven zonder ingevoerde kleur.
        [TestMethod]
        public void TestAverageCalculatorNoValue()
        {
            var reader = new DummyPixelReader();
            IAverageColorCalculator calculator = new AverageColorCalculator(reader);

            var average = calculator.CalculateAverage(null);

            var white = Color.FromArgb(0, 255, 255, 255);
            Assert.AreEqual(white, average);
        }
    }
}