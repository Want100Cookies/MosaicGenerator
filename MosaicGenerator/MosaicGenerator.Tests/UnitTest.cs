﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MosaicGenerator.Tests.Dummy;
using System.Threading.Tasks;
using Windows.UI;
using MosaicGenerator.Implementations;
using MosaicGenerator.Abstractions;
using System.Collections.Generic;

namespace MosaicGenerator.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public async Task TestBlockAverageCalculator()
        {
            var calculator = new AverageColorCalculator();
            var colors = await calculator.CalculateAverage(new DummyImage(), 2);

            Assert.AreEqual(4, colors.Length);
            Assert.AreEqual(Color.FromArgb(0xFF, 0x00, 0x00, 0xFF), colors[0]);
            Assert.AreEqual(Color.FromArgb(0xFF, 0x00, 0xFF, 0x00), colors[1]);
            Assert.AreEqual(Color.FromArgb(0xFF, 0xFF, 0x00, 0x00), colors[2]);
            Assert.AreEqual(Color.FromArgb(0xFF, 0xFF, 0xFF, 0x00), colors[3]);
        }

        //[TestMethod]
        //public async Task TestAverageCalculator()
        //{

        //    //var white = Color.FromArgb(0, 255, 255, 255);
        //    //var reader = new DummyPixelReader(white);
        //    //IAverageColorCalculator calculator = new AverageColorCalculator();

        //    //var average = calculator.CalculateAverage(null);

        //    //Assert.AreEqual(white, average);
        //}

        //// Test of de gemiddelde kleur zwart is voor de ingevoerde kleur zwart.
        //[TestMethod]
        //public async Task TestAverageCalculatorBlack()
        //{
        //    //var black = Color.FromArgb(0, 0, 0, 0);
        //    //var reader = new DummyPixelReader(black);
        //    //IAverageColorCalculator calculator = new AverageColorCalculator();

        //    //var average = calculator.CalculateAverage(null);

        //    //Assert.AreEqual(black, average);
        //}

        //// Test of het gemiddelde wit wordt teruggegeven zonder ingevoerde kleur.
        //[TestMethod]
        //public async Task TestAverageCalculatorNoValue()
        //{
        //    var reader = new DummyPixelReader();
        //    IAverageColorCalculator calculator = new AverageColorCalculator();

        //    var average = calculator.CalculateAverage(null);

        //    var white = Color.FromArgb(0, 255, 255, 255);
        //    Assert.AreEqual(white, average);
        //}

        //[TestMethod]
        //public async Task TestClosestImageSelectorExact()
        //{
        //    IClosestImageSelector imageSelector = new ClosestImageSelector();

        //    var originalColor = Color.FromArgb(0, 255, 255, 255);

        //    var colorDict = new Dictionary<Color, List<IImage>>();

        //    var closestBitmap = new DummyImage(new byte[] {
        //        0xFF, 0x00, 0x00, 0xFF,     0xFF, 0x00, 0x00, 0xFF,     0x00, 0xFF, 0x00, 0xFF,     0x00, 0xFF, 0x00, 0xFF,
        //        0xFF, 0x00, 0x00, 0xFF,     0xFF, 0x00, 0x00, 0xFF,     0x00, 0xFF, 0x00, 0xFF,     0x00, 0xFF, 0x00, 0xFF,
        //        0xFF, 0x00, 0x00, 0xFF,     0xFF, 0x00, 0x00, 0xFF,     0x00, 0xFF, 0x00, 0xFF,     0x00, 0xFF, 0x00, 0xFF,
        //        0xFF, 0x00, 0x00, 0xFF,     0xFF, 0x00, 0x00, 0xFF,     0x00, 0xFF, 0x00, 0xFF,     0x00, 0xFF, 0x00, 0xFF
        //    }, 4, 4);
        //    var closestColor = originalColor;

        //    colorDict.Add(closestColor, new List<SoftwareBitmap> { closestBitmap });

        //    var farBitmap = new SoftwareBitmap(BitmapPixelFormat.Bgra8, 10, 10); // Different size so not equel
        //    var farColor = Color.FromArgb(0, 0, 0, 0);

        //    colorDict.Add(farColor, new List<SoftwareBitmap> { farBitmap });

        //    var farBitmap2 = new SoftwareBitmap(BitmapPixelFormat.Bgra8, 20, 20); // Different size so not equel
        //    var farColor2 = Color.FromArgb(0, 255, 255, 0);

        //    colorDict.Add(farColor2, new List<SoftwareBitmap> { farBitmap2 });

        //    var bitmap = await imageSelector.FindClosestImage(originalColor, colorDict);

        //    Assert.AreEqual(closestBitmap, bitmap);
        //}

        //[TestMethod]
        //public async Task TestClosestImageSelectorNear()
        //{
        //    IClosestImageSelector imageSelector = new ClosestImageSelector();

        //    var originalColor = Color.FromArgb(0, 255, 255, 255);

        //    var colorDict = new Dictionary<Color, List<SoftwareBitmap>>();

        //    var closestBitmap = new SoftwareBitmap(BitmapPixelFormat.Bgra8, 5, 5);
        //    var closestColor = Color.FromArgb(0, 200, 200, 200);

        //    colorDict.Add(closestColor, new List<SoftwareBitmap> { closestBitmap });

        //    var farBitmap = new SoftwareBitmap(BitmapPixelFormat.Bgra8, 10, 10); // Different size so not equel
        //    var farColor = Color.FromArgb(0, 0, 0, 0);

        //    colorDict.Add(farColor, new List<SoftwareBitmap> { farBitmap });

        //    var farBitmap2 = new SoftwareBitmap(BitmapPixelFormat.Bgra8, 20, 20); // Different size so not equel
        //    var farColor2 = Color.FromArgb(0, 255, 255, 0);

        //    colorDict.Add(farColor2, new List<SoftwareBitmap> { farBitmap2 });

        //    var bitmap = await imageSelector.FindClosestImage(originalColor, colorDict);

        //    Assert.AreEqual(closestBitmap, bitmap);
        //}
    }
}