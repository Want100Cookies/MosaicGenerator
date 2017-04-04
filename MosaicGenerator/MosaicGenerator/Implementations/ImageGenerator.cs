using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MosaicGenerator.Abstractions;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using System.Diagnostics;

namespace MosaicGenerator.Implementations
{
    public class ImageGenerator : IImageGenerator
    {
        private readonly IClosestImageSelector closestImageSelector;
        private readonly IAverageColorCalculator averageColorCalculator;
        private IProgress<int> progress;

        public ImageGenerator(IClosestImageSelector closestImageSelector, IAverageColorCalculator averageColorCalculator, IProgress<int> progress)
        {
            this.closestImageSelector = closestImageSelector;
            this.averageColorCalculator = averageColorCalculator;
            this.progress = progress;
        }

        public async Task<WriteableBitmap> GenerateImage(IImage image, IDictionary<Color, List<IImage>> lookup, int blockSize)
        {
            Color[] exampleBlocks = await averageColorCalculator.CalculateAverage(image, blockSize);

            IImage[] closestImages = new IImage[exampleBlocks.Length];

            for (int i = 0; i < exampleBlocks.Length; i++)
            {
                closestImages[i] = await closestImageSelector.FindClosestImage(exampleBlocks[i], lookup);
            }

            int width = await image.GetWidth();
            int height = await image.GetHeigth();
            int cols = width / blockSize;

            WriteableBitmap destBitmap = new WriteableBitmap(width, height);

            for (int i = 0; i < closestImages.Length; i++)
            {
                byte[] currentPixels = await closestImages[i].GetPixels();
                int currentWidth = await closestImages[i].GetWidth();
                int currentHeight = await closestImages[i].GetHeigth();

                WriteableBitmap current = new WriteableBitmap(currentWidth, currentHeight);

                current = WriteableBitmapExtensions.FromByteArray(current, currentPixels);

                int x = (i % cols) * blockSize;
                int y = (i / cols) * blockSize;

                destBitmap.Blit(
                    new Windows.Foundation.Rect(x, y, blockSize, blockSize),
                    current,
                    new Windows.Foundation.Rect(0, 0, current.PixelWidth, current.PixelHeight));

                progress.Report(i);
            }

            return destBitmap;
        }
    }
}