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
        [ComImport]
        [Guid("5b0d3235-4dba-4d44-865e-8f1d0e4fd04d")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        unsafe interface IMemoryBufferByteAccess
        {
            void GetBuffer(out byte* buffer, out uint capacity);
        }

        public unsafe Task<Color[]> GetPixelData(SoftwareBitmap image)
        {
            return Task.Run(() =>
            {
                const int BYTES_PER_PIXEL = 4;

                using (var buffer = image.LockBuffer(BitmapBufferAccessMode.Read))
                {
                    using (var reference = buffer.CreateReference())
                    {
                        byte* data;
                        uint capacity;
                        ((IMemoryBufferByteAccess)reference).GetBuffer(out data, out capacity);

                        var desc = buffer.GetPlaneDescription(0);

                        Color[] colors = new Color[image.PixelHeight * image.PixelWidth];
                        int index = 0;

                        for (uint row = 0; row < desc.Height; row++)
                        {
                            for (uint col = 0; col < desc.Width; col++)
                            {
                                var currPixel = desc.StartIndex + desc.Stride * row + BYTES_PER_PIXEL * col;

                                var b = data[currPixel + 0];
                                var g = data[currPixel + 1];
                                var r = data[currPixel + 2];

                                colors[index++] = Color.FromArgb(255, r, g, b);
                            }
                        }

                        return colors;
                    }
                }
            });
        }
    }
}