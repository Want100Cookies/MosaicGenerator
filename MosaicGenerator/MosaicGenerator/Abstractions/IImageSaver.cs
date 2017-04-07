using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MosaicGenerator.Abstractions
{
    public interface IImageSaver
    {
        Task SaveImageAsync(WriteableBitmap image, StorageFile outputFile);
    }
}