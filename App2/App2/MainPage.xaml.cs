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
using Windows.Storage.Streams;
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

namespace App2
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

        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {

            

            var folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            

            if (folder != null)
            {
                listView.Items.Clear();

                // Application now has read/write access to all contents in the picked folder
                // (including other sub-folder contents)
                Windows.Storage.AccessCache.StorageApplicationPermissions.
                FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                this.textBlock.Text = "Picked folder: " + folder.Name;

                await Task.Run(() =>
                {
                    CalculateAverageColor(folder);
                    
                });

            }
            else
            {
                this.textBlock.Text = "Operation cancelled.";
            }
        }
        private async Task CalculateAverageColor(StorageFolder folder)
        {


            //listView.Items.Clear();

            List<string> fileTypeFilter = new List<string>();
            fileTypeFilter.Add(".jpg");
            fileTypeFilter.Add(".png");
            fileTypeFilter.Add(".bmp");
            QueryOptions queryOptions = new QueryOptions(CommonFileQuery.OrderByName, fileTypeFilter);

            // Create query and retrieve files
            StorageFileQueryResult query = folder.CreateFileQueryWithOptions(queryOptions);
            IReadOnlyList<StorageFile> fileList = await query.GetFilesAsync();

            IStorageFile[] filePaths = fileList.ToArray();

            Stopwatch stopwatch = Stopwatch.StartNew();
            var tasks = filePaths.Select(async filePath =>
            {
                using (IRandomAccessStream stream = await filePath.OpenAsync(FileAccessMode.Read))
                {

                    //Create a decoder for the image
                    var decoder = await BitmapDecoder.CreateAsync(stream);

                    //Create a transform to get a 1x1 image
                    var myTransform = new BitmapTransform();

                    //Get the pixel provider
                    PixelDataProvider pixels = await decoder.GetPixelDataAsync(
                        BitmapPixelFormat.Rgba8,
                        BitmapAlphaMode.Straight,
                        myTransform,
                        ExifOrientationMode.RespectExifOrientation,
                        ColorManagementMode.ColorManageToSRgb);

                    byte[] color = pixels.DetachPixelData();

                    //read the color 
                    int r = 0, g = 0, b = 0;

                    for (int i = 0; i < color.Length; i += 4)
                    {
                        r += color[i ];
                        g += color[i + 1];
                        b += color[i + 2];
                    }

                    float arrayLength = color.Length /4;

                    var myDominantColor = Color.FromArgb(255, (byte)(r / arrayLength), (byte)(g / arrayLength), (byte)(b / arrayLength));

                    Debug.WriteLine( filePath.Name + "-----" + myDominantColor.ToString());
                    
                }
            });
            await Task.WhenAll(tasks);

            stopwatch.Stop();

            Debug.WriteLine ("Done in " + stopwatch.ElapsedMilliseconds + " milliseconds");
        }
    }
}
