using System.Threading.Tasks;
using Windows.Graphics.Imaging;

namespace MosaicGenerator.Abstractions
{
    public interface IImageSaver
    {
        Task SaveImageAsync(IImage image);
    }
}