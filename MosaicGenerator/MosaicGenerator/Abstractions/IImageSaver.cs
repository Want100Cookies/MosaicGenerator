using System.Threading.Tasks;

namespace MosaicGenerator.Abstractions
{
    public interface IImageSaver
    {
        Task SaveImageAsync(IImage image);
    }
}