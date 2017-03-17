using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI;

namespace MosaicGenerator.Abstractions
{
    public interface IImageGenerator
    {
        /// <summary>
        /// Assemble the final mosaic from source images
        /// </summary>
        /// <param name="image">Source image to mosaic over</param>
        /// <param name="lookup">Lookup table of images and average colors to be used in the mosaic</param>
        /// <param name="blockSize">Size in pixels of each block</param>
        /// <returns>Mosaic image</returns>
        Task<SoftwareBitmap> GenerateImage(SoftwareBitmap image, Dictionary<Color, List<SoftwareBitmap>> lookup, int blockSize);
    }
}