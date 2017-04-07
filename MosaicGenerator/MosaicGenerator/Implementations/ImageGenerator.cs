using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MosaicGenerator.Abstractions;
using Windows.UI;
using System.Diagnostics;

namespace MosaicGenerator.Implementations
{
    public class ImageGenerator : IImageGenerator
    {
        private readonly IClosestImageSelector closestImageSelector;
        private readonly IAverageColorCalculator averageColorCalculator;
        private IProgress<int> progress;
        private IImage[] closestImages;
        private byte[] destBytes;

        public ImageGenerator(IClosestImageSelector closestImageSelector, IAverageColorCalculator averageColorCalculator, IProgress<int> progress)
        {
            this.closestImageSelector = closestImageSelector;
            this.averageColorCalculator = averageColorCalculator;
            this.progress = progress;
        }

        public async Task<byte[]> GenerateImage(IImage image, IDictionary<Color, List<IImage>> lookup, int blockSize)
        {
            Color[] exampleBlocks = await averageColorCalculator.CalculateAverage(image, blockSize);

            closestImages = new IImage[exampleBlocks.Length];

            for (int i = 0; i < exampleBlocks.Length; i++)
            {
                closestImages[i] = await closestImageSelector.FindClosestImage(exampleBlocks[i], lookup);
            }

            int width = await image.GetWidth();
            int height = await image.GetHeight();
            int cols = width / blockSize;

            destBytes = new byte[width * height * 4];

            var blitTasks = new List<Task>();
            int chunkSize = closestImages.Length / Environment.ProcessorCount;

            for (int t = 0; t < Environment.ProcessorCount; t++)
            {
                int _t = t;
                blitTasks.Add(Task.Run(async () =>
                {
                    var target = closestImages.Length - (Environment.ProcessorCount - _t - 1) * chunkSize;
                    if(_t == Environment.ProcessorCount - 1)
                    {
                        target = closestImages.Length;
                    }

                    Debug.WriteLine($"starting blit task {_t} for {_t * chunkSize} to {target}");

                    for(int i = _t * chunkSize; i < target; i++)
                    {
                        byte[] currentPixels = await closestImages[i].GetResizedPixels(blockSize, blockSize);

                        int x = (i % cols) * blockSize;
                        int y = (i / cols) * blockSize;
                        int byteBlockWidth = blockSize * 4;

                        for (int j = 0; j < blockSize; j++)
                        {
                            int destinationIndex = x * 4 + y * 4 * width;
                            int sourceIndex = j * 4 * blockSize;

                            y++;

                            Array.Copy(currentPixels, sourceIndex, destBytes, destinationIndex, byteBlockWidth);
                        }

                        progress.Report(i);
                    }
                }));
            }

            await Task.WhenAll(blitTasks);

            return destBytes;
        }
    }
}