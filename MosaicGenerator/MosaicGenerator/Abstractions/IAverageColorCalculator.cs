using System.Threading.Tasks;
using Windows.UI;

namespace MosaicGenerator.Abstractions
{
    public interface IAverageColorCalculator
    {
        Task<Color> CalculateAverage(Color[] pixelData);
    }
}