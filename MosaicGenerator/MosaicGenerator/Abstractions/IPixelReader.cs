using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI;

namespace MosaicGenerator.Abstractions
{
    public interface IPixelReader
    {
        /// <summary>
        /// Read pixel data from a SoftwareBitmap
        /// </summary>
        /// <param name="image">SoftwareBitmap to read from</param>
        /// <returns>Array of color data</returns>
        Task<Color[]> GetPixelData(SoftwareBitmap image);
    }
}