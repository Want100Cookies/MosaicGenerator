using System.Threading.Tasks;
using Windows.UI;

namespace MosaicGenerator.Abstractions
{
    public interface IImage
    {
        Task<byte[]> GetPixels();
        Task<byte[]> GetResizedPixels(int width, int height);
        string GetFileName();
        Task<int> GetWidth();
        Task<int> GetHeight();
    }
}