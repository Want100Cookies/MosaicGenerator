﻿using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI;

namespace MosaicGenerator.Abstractions
{
    public interface IImageReader
    {
        Task<SoftwareBitmap> ReadImageAsync(string path);
    }
}