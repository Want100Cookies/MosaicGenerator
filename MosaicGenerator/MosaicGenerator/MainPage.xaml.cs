using MosaicGenerator.Abstractions;
using MosaicGenerator.Implementations;
using System;
using System.Collections.Concurrent;
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
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

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
            IFolderReader folderReader = new FolderReader();
            StorageFolder folder = await folderReader.PickFolderAsync();

            if (folder != null)
            {
                IStorageFile[] filePaths = await folderReader.ReadFolderAsync(folder);

                progressBar.Maximum = filePaths.Length;

                Stopwatch stopwatch = Stopwatch.StartNew();
                IDictionary<Color, List<string>> averageColors;

                await Task.Run(async () =>
                {
                    averageColors = await calculate(filePaths);
                });

                GC.Collect();
                stopwatch.Stop();

                await new MessageDialog("Done in " + stopwatch.ElapsedMilliseconds + " milliseconds").ShowAsync();
            }

        }

        private async Task<IDictionary<Color, List<string>>> calculate(IStorageFile[] files)
        {
            ConcurrentDictionary<Color, List<string>> images = new ConcurrentDictionary<Color, List<string>>();

            IAverageColorCalculator calculator = new AverageColorCalculator();


            var tasks = files.Select(async file =>
            {
                IImage image = new Implementations.Image(file);
                Color average = calculator.CalculateAverage(await image.GetPixels());

                Debug.WriteLine("Found " + average.ToString() + " in " + image.GetFileName());

                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        progressBar.Value++;
                    });

                List<string> imageWithColor;
                if (images.TryGetValue(average, out imageWithColor))
                {
                    imageWithColor.Add(file.Path);
                }
                else
                {
                    images.TryAdd(average, new List<string>() { file.Path });
                }

            });

            await Task.WhenAll(tasks);

            return images;
        }

        private async void BtnSelectSourceClick(object sender, RoutedEventArgs e)
        {
            IAverageColorCalculator calculator = new AverageColorCalculator();

            var picker = new FileOpenPicker()
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };

            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                IImage image = new Implementations.Image(file);

                int max = 5;
                int size = 100;

                Color[] colors = await calculator.CalculateAverage(image, size);


                int i = 0;

                foreach (Color color in colors)
                {
                    Rectangle rectangle = new Rectangle()
                    {
                        Fill = new SolidColorBrush(color),
                        Width = 100,
                        Height = 100
                    };
                    
                    outputGrid.Children.Add(rectangle);

                    int y = i % max;
                    int x = i / max;

                    Grid.SetColumn(rectangle, x);
                    Grid.SetRow(rectangle, y);

                    Debug.WriteLine($"Set {color.ToString()} at {x}x{y}");
                    i++;                    
                }
            }
        }
    }
}
