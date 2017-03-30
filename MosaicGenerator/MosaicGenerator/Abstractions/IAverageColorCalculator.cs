using Windows.Graphics.Imaging;
using Windows.UI;

namespace MosaicGenerator.Abstractions
{
    public interface IAverageColorCalculator
    {
        /// <summary>
        /// Calculate the average color of an entire image
        /// </summary>
        /// <param name="image">Image to calculate the average color of</param>
        /// <returns>The average color of the entire image</returns>
        Color CalculateAverage(SoftwareBitmap image);

        /// <summary>
        /// Calculate the average color of each section of the image
        /// If blocks fall outside the dimensions their pixels shouldn't be counted towards the average
        /// </summary>
        /// <param name="image">Image to calculate the average block colors of</param>
        /// <param name="blockSize">Size in pixels of each block</param>
        /// <returns>An array of the average colors of the blocks</returns>
        Color[] CalculateAverage(SoftwareBitmap image, int blockSize);
    }
}