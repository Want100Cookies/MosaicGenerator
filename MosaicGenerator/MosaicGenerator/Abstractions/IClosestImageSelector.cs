using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI;

namespace MosaicGenerator.Abstractions
{
    public interface IClosestImageSelector
    {
        Task<SoftwareBitmap> FindClosestImage(Color color, Dictionary<Color, List<SoftwareBitmap>> images);
    }
}