using MosaicGenerator.Abstractions;
using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace MosaicGenerator.Implementations
{
    class ImageSaver : IImageSaver
    {
        public async Task SaveImageAsync(WriteableBitmap image, StorageFile outputFile)
        {
            Guid BitmapEncoderGuid = BitmapEncoder.PngEncoderId;
            using (IRandomAccessStream stream = await outputFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoderGuid, stream);
                Stream pixelStream = image.PixelBuffer.AsStream();
                byte[] pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);

                encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                    BitmapAlphaMode.Ignore,
                                    (uint)image.PixelWidth,
                                    (uint)image.PixelHeight,
                                    96.0,
                                    96.0,
                                    pixels);

                await encoder.FlushAsync();
            }
        }
    }
}