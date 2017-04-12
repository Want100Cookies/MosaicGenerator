using MosaicGenerator.Abstractions;
using System.Threading.Tasks;
using Windows.UI;

namespace MosaicGenerator.Implementations
{
    public class AdjustedImage : IImage
    {
        private IImage original;
        private Color color;
        private float amount;

        public AdjustedImage(IImage original, Color color, float amount)
        {
            this.original = original;
            this.color = color;
            this.amount = amount;
        }

        public string GetFileName() => original.GetFileName();
        public Task<int> GetWidth() => original.GetWidth();
        public Task<int> GetHeight() => original.GetHeight();

        private byte Lerp(byte a, byte b, float t)
        {
            return (byte)((1 - t) * a + t * b);
        }

        public byte[] AdjustPixels(byte[] pixels)
        {
            // BGRA

            for (int i = 0; i < pixels.Length; i += 4)
            {
                pixels[i] = Lerp(pixels[i], color.B, amount);
                pixels[i + 1] = Lerp(pixels[i + 1], color.G, amount);
                pixels[i + 2] = Lerp(pixels[i + 2], color.R, amount);
                //pixels[i + 3] = 0;
            }

            return pixels;
        }

        public async Task<byte[]> GetPixels()
        {
            return AdjustPixels(await original.GetPixels());
        }

        public async Task<byte[]> GetResizedPixels(int width, int height)
        {
            return AdjustPixels(await original.GetResizedPixels(width, height));
        }
    }
}