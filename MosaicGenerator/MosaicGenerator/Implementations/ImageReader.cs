using MosaicGenerator.Abstractions;
using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

namespace MosaicGenerator.Implementations
{
    public class ImageReader : IImageReader
    {
        /*
        public async Task<Color[]> ReadImageAsync(string path)
        {
            using (Stream imageStream = await Task.Run(() => File.OpenRead(path)))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(imageStream.AsRandomAccessStream());

                PixelDataProvider data = await decoder.GetPixelDataAsync(
                    BitmapPixelFormat.Rgba8,
                    BitmapAlphaMode.Ignore,
                    new BitmapTransform()
                    {
                        ScaledHeight = decoder.PixelHeight,
                        ScaledWidth = decoder.PixelWidth
                    },
                    ExifOrientationMode.IgnoreExifOrientation,
                    ColorManagementMode.DoNotColorManage
                    );

                byte[] imageBytes = data.DetachPixelData();
                Color[] imageColors = new Color[imageBytes.Length];
                int j = 0;

                for (int i = 0; i < imageBytes.Length; i += 4)
                {
                    byte r = imageBytes[i];
                    byte g = imageBytes[i + 1];
                    byte b = imageBytes[i + 2];

                    imageColors[j++] = Color.FromArgb(0, r, g, b);
                }


                return imageColors;
            }
        }
        */

        public Task<SoftwareBitmap> ReadImageAsync(string path)
        {
            throw new NotImplementedException();
        }
    }
}