using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MosaicGenerator.Abstractions;
using Windows.Graphics.Imaging;
using Windows.UI;

namespace MosaicGenerator.Implementations
{
    public class ImageGenerator : IImageGenerator
    {
        private readonly IClosestImageSelector closestImageSelector;
        private readonly IAverageColorCalculator averageColorCalculator;

        public ImageGenerator(IClosestImageSelector closestImageSelector, IAverageColorCalculator averageColorCalculator)
        {
            this.closestImageSelector = closestImageSelector;
            this.averageColorCalculator = averageColorCalculator;
        }

        public async Task<IImage> GenerateImage(IImage image, Dictionary<Color, List<IImage>> lookup, int blockSize)
        {
            //var output = new SoftwareBitmap(BitmapPixelFormat.Rgba8, image.PixelWidth, image.PixelHeight);

            ////var blocks = await averageColorCalculator.CalculateAverage(image, blockSize);

            //for(int i = 0; i < blocks.Length; i++)
            //{
            //    var closest = await closestImageSelector.FindClosestImage(blocks[i], lookup);
            //}

            //return output;

            return null;
        }
    }
}