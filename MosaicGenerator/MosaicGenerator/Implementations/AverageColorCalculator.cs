using System;
using System.Threading.Tasks;
using MosaicGenerator.Abstractions;
using Windows.Graphics.Imaging;
using Windows.UI;

namespace MosaicGenerator.Implementations
{
    public class AverageColorCalculator : IAverageColorCalculator
    {
        private readonly IPixelReader pixelReader;

        public AverageColorCalculator(IPixelReader pixelReader)
        {
            this.pixelReader = pixelReader;
        }

        /*
        public Task<Color> CalculateAverage(Color[] pixelData)
        {
            int r = 0, g = 0, b = 0;

            for (int i = 0; i < pixelData.Length; i++)
            {
                r += pixelData[i].R;
                g += pixelData[i].G;
                b += pixelData[i].B;
            }

            return Task.FromResult(Color.FromArgb(0, (byte)(r / (float)pixelData.Length), (byte)(g / (float)pixelData.Length), (byte)(b / (float)pixelData.Length)));
        }
        */

        public Task<Color> CalculateAverage(SoftwareBitmap image)
        {
            throw new NotImplementedException();
        }

        public Task<Color[]> CalculateAverage(SoftwareBitmap image, int blockSize)
        {
            throw new NotImplementedException();
        }
    }
}