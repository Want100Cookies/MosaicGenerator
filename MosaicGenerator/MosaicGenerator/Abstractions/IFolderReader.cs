using System.Threading.Tasks;
using Windows.Storage;

namespace MosaicGenerator.Abstractions
{
    public interface IFolderReader
    {
        /// <summary>
        /// Allow the user to pick a folder
        /// </summary>
        /// <returns></returns>
        Task<StorageFolder> PickFolderAsync();

        /// <summary>
        /// Get a list of all files in specified folder
        /// </summary>
        /// <param name="folder">Folder to read from</param>
        /// <returns>Found files in folder</returns>
        Task<IStorageFile[]> ReadFolderAsync(StorageFolder folder);
    }
}