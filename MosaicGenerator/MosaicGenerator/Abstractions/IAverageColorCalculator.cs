﻿using System.Threading.Tasks;
using Windows.UI;

namespace MosaicGenerator.Abstractions
{
    public interface IAverageColorCalculator
    {
        /// <returns>The average color of the entire image</returns>
        Color CalculateAverage(byte[] colors);

        /// <returns>An array of the average colors of the blocks</returns>
        Task<Color[]> CalculateAverage(IImage image, int blockSize);
    }
}