using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI;

namespace MosaicGenerator.Abstractions
{
    public interface IImageGenerator
    {
        Task<SoftwareBitmap> GenerateImage(SoftwareBitmap[] images, Dictionary<Color, List<SoftwareBitmap>> lookup, int blockSize);
    }
}