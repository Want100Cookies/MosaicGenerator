using System.Threading.Tasks;
using Windows.UI;

namespace MosaicGenerator.Abstractions
{
    public interface IImageReader
    {
        Task<Color[]> ReadImageAsync(string path);
    }
}