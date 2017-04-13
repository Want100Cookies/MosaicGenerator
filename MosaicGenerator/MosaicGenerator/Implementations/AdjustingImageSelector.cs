using MosaicGenerator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;

namespace MosaicGenerator.Implementations
{
    public class AdjustingImageSelector : IClosestImageSelector
    {
        private static Random random = new Random();

        private float amount;

        public AdjustingImageSelector(float amount)
        {
            this.amount = amount;
        }

        public Task<IImage> FindClosestImage(Color color, IDictionary<Color, List<IImage>> images)
        {
            return Task.Run(() =>
            {
                List<IImage> imageList;
                if (!images.TryGetValue(color, out imageList))
                {
                    List<Color> allColors = images.Keys.ToList();

                    int colorDiff = allColors.Select(n => ColorDiff(n, color)).Min(n => n);
                    Color closestColor = allColors.First(n => ColorDiff(n, color) == colorDiff);

                    imageList = images[closestColor];
                }

                int index = random.Next(imageList.Count); 
                var image = imageList[index];

                return (IImage)new AdjustedImage(image, color, amount);
            });
        }

        private int ColorDiff(Color c1, Color c2)
        {
            return (int)Math.Sqrt((c1.R - c2.R) * (c1.R - c2.R)
                + (c1.G - c2.G) * (c1.G - c2.G)
                + (c1.B - c2.B) * (c1.B - c2.B));
        }
    }
}