using System;
using System.Threading.Tasks;
using MosaicGenerator.Abstractions;
using Windows.UI;

namespace MosaicGenerator.Tests.Dummy
{
    public class DummyImageReader : IImageReader
    {
        public Color Color { get; set; }

        public DummyImageReader(Color? color = null)
        {
            color = color ?? Color.FromArgb(0, 255, 255, 255);
            Color = color.Value;
        }

        public Task<Color[]> ReadImage(string path)
        {
            return Task.FromResult(new Color[] { Color, Color, Color, Color });
        }
    }
}