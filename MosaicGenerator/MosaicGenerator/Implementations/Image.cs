using MosaicGenerator.Abstractions;
using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;

namespace MosaicGenerator.Implementations
{
    class Image : IImage
    {
        private IPixelReader pixelReader;
        private IStorageFile file;

        private Color color;
        private float amount = 0f;

        public Image(IStorageFile file)
        {
            this.file = file;
            this.pixelReader = new PixelReader();
        }

        public void Adjust(Color color, float amount)
        {
            this.color = color;
            this.amount = amount;
        }

        public string GetFileName()
        {
            return file.Path;
        }

        public async Task<byte[]> GetPixels()
        {
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

                return await pixelReader.GetPixelData(decoder);
            }
        }

        public async Task<int> GetHeight()
        {
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

                return (int)decoder.PixelHeight;
            }
        }

        public async Task<int> GetWidth()
        {
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

                return (int)decoder.PixelWidth;
            }
        }

        public async Task<byte[]> GetResizedPixels(int width, int height)
        {
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

                return await pixelReader.GetResizedPixelData(decoder, width, height);
            }
        }
    }
}