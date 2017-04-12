using MosaicGenerator.Abstractions;
using System.Threading.Tasks;
using System;
using Windows.UI;

namespace MosaicGenerator.Tests.Dummy
{
    public class DummyImage : IImage
    {
        public string GetFileName() => "Dummy";

        public Task<int> GetWidth() => Task.FromResult(width);
        public Task<int> GetHeight() => Task.FromResult(width);

        private int width, height;
        private byte[] data;

        public DummyImage()
        {
            width = 4;
            height = 4;

            // BGRA
            data = new byte[]
            {
                0xFF, 0x00, 0x00, 0xFF,     0xFF, 0x00, 0x00, 0xFF,     0x00, 0xFF, 0x00, 0xFF,     0x00, 0xFF, 0x00, 0xFF,
                0xFF, 0x00, 0x00, 0xFF,     0xFF, 0x00, 0x00, 0xFF,     0x00, 0xFF, 0x00, 0xFF,     0x00, 0xFF, 0x00, 0xFF,
                0x00, 0x00, 0xFF, 0xFF,     0x00, 0x00, 0xFF, 0xFF,     0x00, 0xFF, 0xFF, 0xFF,     0x00, 0xFF, 0xFF, 0xFF,
                0x00, 0x00, 0xFF, 0xFF,     0x00, 0x00, 0xFF, 0xFF,     0x00, 0xFF, 0xFF, 0xFF,     0x00, 0xFF, 0xFF, 0xFF
            };
        }

        public DummyImage(byte[] data, int width, int height)
        {
            this.width = width;
            this.height = height;
            this.data = data;
        }

        public Task<byte[]> GetPixels()
        {
            return Task.FromResult(data);
        }

        public Task<byte[]> GetResizedPixels(int width, int height)
        {
            throw new NotImplementedException();
        }
    }
}