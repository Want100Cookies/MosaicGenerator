using System.Collections.Generic;
using Windows.Graphics.Imaging;
using Windows.UI;

namespace MosaicGenerator.Abstractions
{
    public interface IClosestImageSelector
    {
        /// <summary>
        /// Select the best matching image for average color
        /// </summary>
        /// <param name="color">Average color to fill</param>
        /// <param name="images">Lookup table of images</param>
        /// <returns>Image to use</returns>
        Task<IImage> FindClosestImage(Color color, Dictionary<Color, List<IImage>> images);
    }
}