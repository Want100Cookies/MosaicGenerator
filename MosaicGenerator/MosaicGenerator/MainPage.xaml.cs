using MosaicGenerator.Implementations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
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
            FolderReader folderReader = new FolderReader();
            StorageFolder folder = await folderReader.PickFolderAsync();


            if (folder != null)
            {
                string[] filePaths = await folderReader.ReadFolderAsync(folder);

                ImageReader imageReader = new ImageReader();
                AverageColorCalculator calculator = new AverageColorCalculator(new PixelReader());

                Dictionary<Color, List<string>> files = new Dictionary<Color, List<string>>();

                Stopwatch stopwatch = Stopwatch.StartNew();
                
                var tasks = filePaths.Select(async filePath =>
                {
                    SoftwareBitmap image = await imageReader.ReadImageAsync(filePath);
                    Color average = await calculator.CalculateAverage(image);

                    Debug.WriteLine("Done: " + filePath);

                    List<string> filesWithColor;
                    if (files.TryGetValue(average, out filesWithColor))
                    {
                        filesWithColor.Add(filePath);
                    }
                    else
                    {
                        files.Add(average, new List<string>() { filePath });
                    }
                });

                await Task.WhenAll(tasks);

                foreach (KeyValuePair<Color, List<string>> fileWithColor in files)
                {
                    foreach (string path in fileWithColor.Value)
                    {
                        ImageList.Items.Add(new TextBlock()
                        {
                            Text = $"{fileWithColor.Key.ToString()} in {path}"
                        });
                    }
                }

                stopwatch.Stop();

                await new MessageDialog("Done in " + stopwatch.ElapsedMilliseconds + " milliseconds").ShowAsync();
            }

        }
    }
}
