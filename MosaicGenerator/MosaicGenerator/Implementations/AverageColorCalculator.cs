using System;
using System.Threading.Tasks;
using MosaicGenerator.Abstractions;
using Windows.Graphics.Imaging;
using Windows.UI;

namespace MosaicGenerator.Implementations
{
    public class AverageColorCalculator : IAverageColorCalculator
    {
        public Color CalculateAverage(byte[] colors)
        {
            //read the color 
            int r = 0, g = 0, b = 0;

            for (int i = 0; i < colors.Length; i += 4)
            {
                r += colors[i];
                g += colors[i + 1];
                b += colors[i + 2];
            }

            float arrayLength = colors.Length / 4;

            return Color.FromArgb(255, (byte)(r / arrayLength), (byte)(g / arrayLength), (byte)(b / arrayLength));
        }

        public Color[] CalculateAverage(IImage image, int blockSize)
        {
            throw new NotImplementedException();
        }
    }
}