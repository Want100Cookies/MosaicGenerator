using MosaicGenerator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI;

namespace MosaicGenerator.Implementations
{
    class ClosestImageSelector : IClosestImageSelector
    {
        private static Random random = new Random();

        public Task<SoftwareBitmap> FindClosestImage(Color color, Dictionary<Color, List<SoftwareBitmap>> images)
        {
            // Todo: Make unit test
            return Task.Run(() =>
            {
                List<SoftwareBitmap> imageList;
                if (!images.TryGetValue(color, out imageList))
                {
                    List<Color> allColors = images.Keys.ToList();

                    int colorDiff = allColors.Select(n => ColorDiff(n, color)).Min(n => n);
                    Color closestColor = allColors.First(n => ColorDiff(n, color) == colorDiff);

                    imageList = images[closestColor];
                }

                int index = random.Next(imageList.Count);
                return imageList[index];
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
