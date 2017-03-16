using System.Threading.Tasks;
using MosaicGenerator.Abstractions;
using Windows.Graphics.Imaging;
using Windows.UI;

namespace MosaicGenerator.Tests.Dummy
{
    public class DummyPixelReader : IPixelReader
    {
        public Color Color { get; set; }

        public DummyPixelReader(Color? color = null)
        {
            color = color ?? Color.FromArgb(0, 255, 255, 255);
            Color = color.Value;
        }

        public Task<Color[]> GetPixelData(SoftwareBitmap image)
        {
            return Task.FromResult(new Color[] { Color, Color, Color, Color });
        }
    }
}