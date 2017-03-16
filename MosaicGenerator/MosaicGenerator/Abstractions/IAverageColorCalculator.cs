using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI;

namespace MosaicGenerator.Abstractions
{
    public interface IAverageColorCalculator
    {
        Task<Color> CalculateAverage(SoftwareBitmap image);

        Task<Color[]> CalculateAverage(SoftwareBitmap image, int width, int blockSize);
    }
}