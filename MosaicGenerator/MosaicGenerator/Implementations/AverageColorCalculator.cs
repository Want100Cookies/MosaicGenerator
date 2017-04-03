﻿using System;
using System.Threading.Tasks;
using MosaicGenerator.Abstractions;
using Windows.Graphics.Imaging;
using Windows.UI;
using System.Collections.Generic;
using System.Diagnostics;

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

        public async Task<Color[]> CalculateAverage(IImage image, int blockSize)
        {
            byte[] pixels = await image.GetPixels();
            int width = await image.GetWidth();
            int height = await image.GetHeigth();

            List<Color> colors = new List<Color>();

            for (int i = 0; i < width; i += blockSize)
            {
                for (int j = height - blockSize; j > -blockSize; j -= blockSize)
                {
                    int total = 0;
                    float R = 0;
                    float G = 0;
                    float B = 0;

                    for (int k = 0; k < blockSize; k++)
                    {
                        for (int l = 0; l < blockSize; l++)
                        {
                            int x = i + k;
                            int y = j + l;

                            int pixelLocation = (x + y) * 4;

                            total++;

                            R += pixels[pixelLocation];
                            G += pixels[pixelLocation + 1];
                            B += pixels[pixelLocation + 2];
                        }
                    }

                    total = total / 2;

                    Color color = Color.FromArgb(
                        255,
                        (byte)(R / total),
                        (byte)(G / total),
                        (byte)(B / total)
                        );

                    colors.Add(color);
                }
            }


            return colors.ToArray();
        }
    }
}