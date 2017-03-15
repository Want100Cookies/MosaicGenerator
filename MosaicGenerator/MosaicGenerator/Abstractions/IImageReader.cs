using System.Threading.Tasks;
using Windows.UI;

namespace MosaicGenerator.Abstractions
{
    public interface IImageReader
    {
        Task<Color[]> ReadImage(string path);
    }
}