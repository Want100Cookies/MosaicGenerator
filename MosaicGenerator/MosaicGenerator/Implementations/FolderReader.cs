using MosaicGenerator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Search;

namespace MosaicGenerator.Implementations
{
    class FolderReader : IFolderReader
    {
        public async Task<StorageFolder> PickFolderAsync()
        {
            FolderPicker folderPicker = new FolderPicker();

            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            return await folderPicker.PickSingleFolderAsync();
        }

        public async Task<string[]> ReadFolderAsync(StorageFolder folder)
        {
            // Application now has read/write access to all contents in the picked folder
            // (including other sub-folder contents)
            Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);

            List<string> fileTypeFilter = new List<string>();
            fileTypeFilter.Add(".jpg");
            fileTypeFilter.Add(".png");
            fileTypeFilter.Add(".bmp");
            QueryOptions queryOptions = new QueryOptions(CommonFileQuery.OrderByName, fileTypeFilter);

            // Create query and retrieve files
            StorageFileQueryResult query = folder.CreateFileQueryWithOptions(queryOptions);
            IReadOnlyList<StorageFile> fileList = await query.GetFilesAsync();

            return fileList.Select(file => file.Path).ToArray();
        }
    }
}
