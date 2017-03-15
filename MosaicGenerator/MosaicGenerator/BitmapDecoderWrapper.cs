using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI;

namespace MosaicGenerator
{
    class BitmapDecoderWrapper
    {
        public BitmapDecoder Decoder { get; }

        public BitmapDecoderWrapper(BitmapDecoder decoder)
        {
            this.Decoder = decoder;
        }

        public async Task<byte[]> GetRawPixelData()
        {
            PixelDataProvider data = await Decoder.GetPixelDataAsync(
                BitmapPixelFormat.Rgba8,
                BitmapAlphaMode.Ignore,
                new BitmapTransform()
                {
                    ScaledHeight = Decoder.PixelHeight,
                    ScaledWidth = Decoder.PixelWidth
                },
                ExifOrientationMode.IgnoreExifOrientation,
                ColorManagementMode.DoNotColorManage
                );

            return data.DetachPixelData();
        }

        public Color GetPixel(byte[] pixels, int x, int y)
        {
            int z = (x * (int) Decoder.PixelWidth + y) * 3;

            byte r = pixels[z + 0];
            byte g = pixels[z + 1];
            byte b = pixels[z + 2];

            return Color.FromArgb(0, r, g, b);
        }

        public async Task<Color> GetPixel(int x, int y)
        {
            byte[] pixels = await GetRawPixelData();

            return GetPixel(pixels, x, y);
        }

        public async Task<Color> GetAverageColor()
        {
            byte[] pixels = await GetRawPixelData();

            int total = (int) Decoder.PixelWidth * (int) Decoder.PixelHeight;

            int r = 0;
            int g = 0;
            int b = 0;

            for (int i = 0; i < pixels.Length; i += 4)
            {
                r += pixels[i];
                g += pixels[i + 1];
                b += pixels[i + 2];
            }

            r = r / total;
            g = g / total;
            b = b / total;

            Color color = Color.FromArgb(0, (byte)r, (byte)g, (byte)b); ;

            Debug.WriteLine("Calculated " + (byte)r + " " + (byte)g + " " + (byte)b);

            return color;
        }
    }
}
