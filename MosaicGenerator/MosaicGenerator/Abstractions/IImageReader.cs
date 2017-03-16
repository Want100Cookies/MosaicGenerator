using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;

namespace MosaicGenerator.Abstractions
{
    public interface IImageReader
    {
        /// <summary>
        /// Read a SoftwareBitmap from a file
        /// </summary>
        /// <param name="file">File to read</param>
        /// <returns>SoftwareBitmap</returns>
        Task<SoftwareBitmap> ReadImageAsync(IStorageFile file);
    }
}