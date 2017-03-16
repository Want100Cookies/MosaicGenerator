using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI;

namespace MosaicGenerator.Abstractions
{
    public interface IPixelReader
    {
        Task<Color[]> GetPixelData(SoftwareBitmap image);
    }
}