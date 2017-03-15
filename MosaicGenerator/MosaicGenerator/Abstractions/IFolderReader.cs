using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MosaicGenerator.Abstractions
{
    public interface IFolderReader
    {
        Task<StorageFolder> PickFolderAsync();

        Task<string[]> ReadFolderAsync(StorageFolder folder);
    }
}
