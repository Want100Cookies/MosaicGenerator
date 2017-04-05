using MosaicGenerator.Abstractions;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI;

namespace MosaicGenerator.Implementations
{
    public class PixelReader : IPixelReader
    {
        public async Task<byte[]> GetPixelData(BitmapDecoder decoder)
        {
            //Get the pixel provider
            PixelDataProvider pixels = await decoder.GetPixelDataAsync(
                BitmapPixelFormat.Bgra8,
                BitmapAlphaMode.Straight,
                new BitmapTransform(),
                ExifOrientationMode.RespectExifOrientation,
                ColorManagementMode.ColorManageToSRgb);

            return pixels.DetachPixelData();
        }
    }
}