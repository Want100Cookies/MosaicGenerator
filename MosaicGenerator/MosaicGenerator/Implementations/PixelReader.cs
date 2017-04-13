using MosaicGenerator.Abstractions;
using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

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

        public async Task<byte[]> GetResizedPixelData(BitmapDecoder decoder, int width, int height)
        {
            //Get the pixel provider
            PixelDataProvider pixels = await decoder.GetPixelDataAsync(
                BitmapPixelFormat.Bgra8,
                BitmapAlphaMode.Straight,
                new BitmapTransform()
                {
                    ScaledWidth = (uint)width,
                    ScaledHeight = (uint)height,
                    InterpolationMode = BitmapInterpolationMode.Fant
                },
                ExifOrientationMode.RespectExifOrientation,
                ColorManagementMode.ColorManageToSRgb);

            return pixels.DetachPixelData();
        }
    }
}