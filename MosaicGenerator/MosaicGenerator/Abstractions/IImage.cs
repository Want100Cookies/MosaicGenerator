using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicGenerator.Abstractions
{
    public interface IImage
    {
        Task<byte[]> GetPixels();
        string GetFileName();
        Task<int> GetWidth();
        Task<int> GetHeigth();
    }
}
