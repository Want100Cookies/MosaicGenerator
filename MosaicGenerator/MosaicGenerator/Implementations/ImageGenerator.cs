using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MosaicGenerator.Abstractions;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using System.Diagnostics;
using System.IO;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;

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

        public async Task<byte[]> GenerateImage(IImage image, IDictionary<Color, List<IImage>> lookup, int blockSize)
        {
            Color[] exampleBlocks = await averageColorCalculator.CalculateAverage(image, blockSize);

            IImage[] closestImages = new IImage[exampleBlocks.Length];

            for (int i = 0; i < exampleBlocks.Length; i++)
            {
                closestImages[i] = await closestImageSelector.FindClosestImage(exampleBlocks[i], lookup);
            }

            int width = await image.GetWidth();
            int height = await image.GetHeight();
            int cols = width / blockSize;

            byte[] destBytes = new byte[width * height * 4];

            //await Task.Run(async () =>
            //{
            for (int i = 0; i < closestImages.Length; i++)
            {
                byte[] currentPixels = await closestImages[i].GetResizedPixels(blockSize, blockSize);
                
                int x = (i % cols) * blockSize;
                int y = (i / cols) * blockSize;

                Debug.WriteLine($"X={x} Y={y} Cols={cols}");

                for (int j = 0; j < blockSize; j++)
                {
                    int index = x * 4 + y * 4 * width;
                    y++;

                    Array.Copy(currentPixels, j * blockSize, destBytes, index, blockSize);
                }


                progress.Report(i);
            }
            //});


            return destBytes;
        }
    }
}