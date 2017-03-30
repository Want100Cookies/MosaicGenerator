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

        public Color CalculateAverage(SoftwareBitmap image)
        {
            Color[] pixelData = pixelReader.GetPixelData(image);

            int r = 0, g = 0, b = 0;

            for (int i = 0; i < pixelData.Length; i++)
            {
                r += pixelData[i].R;
                g += pixelData[i].G;
                b += pixelData[i].B;
            }

            return Color.FromArgb(0, (byte)(r / (float)pixelData.Length), (byte)(g / (float)pixelData.Length), (byte)(b / (float)pixelData.Length));
        }

        public Color[] CalculateAverage(SoftwareBitmap image, int blockSize)
        {
            throw new NotImplementedException();
        }
    }
}