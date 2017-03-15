using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Search;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MosaicGenerator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker folderPicker = new FolderPicker();

            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            if (folder != null)
            {
                // Application now has read/write access to all contents in the picked folder
                // (including other sub-folder contents)
                Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);

                List<string> fileTypeFilter = new List<string>();
                fileTypeFilter.Add(".jpg");
                fileTypeFilter.Add(".png");
                fileTypeFilter.Add(".bmp");
                var queryOptions = new QueryOptions(CommonFileQuery.OrderByName, fileTypeFilter);

                // Create query and retrieve files
                var query = folder.CreateFileQueryWithOptions(queryOptions);
                IReadOnlyList<StorageFile> fileList = await query.GetFilesAsync();

                ILookup<Color, StorageFile> images = fileList
                    .ToLookup(file =>
                    {
                        Stream imageStream = file.OpenStreamForReadAsync().Result;

                        Color average = new Color();

                        var runSync = Task.Factory.StartNew(async () =>
                        {
                            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(imageStream.AsRandomAccessStream());

                            BitmapDecoderWrapper wrapper = new BitmapDecoderWrapper(decoder);
                            average = await wrapper.GetAverageColor();

                        }).Unwrap();

                        runSync.Wait();

                        return average;
                    }, file => file);


                foreach (IGrouping<Color, StorageFile> color in images)
                {

                    foreach (StorageFile file in color)
                    {
                        Debug.WriteLine($"File: {file.Name}, Color: {color.Key.ToString()} {color.Key.R} {color.Key.G} {color.Key.B}");
                    }
                }

                await new MessageDialog("Done").ShowAsync();
            }
            else
            {
                await new MessageDialog("Canceled").ShowAsync();
            }
        }
    }
}
