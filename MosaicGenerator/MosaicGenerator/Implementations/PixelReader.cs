using MosaicGenerator.Abstractions;
using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI;

namespace MosaicGenerator.Implementations
{
    public class PixelReader : IPixelReader
    {
        public Task<Color[]> GetPixelData(SoftwareBitmap image)
        {
            throw new NotImplementedException();
        }
    }
}