using MosaicGenerator.Abstractions;
using MosaicGenerator.Implementations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        private IDictionary<Color, List<IImage>> averageColors;

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
                SelectSrcBtn.IsEnabled = false;

                IStorageFile[] filePaths = await folderReader.ReadFolderAsync(folder);

                progressBar.Maximum = filePaths.Length;

                Stopwatch stopwatch = Stopwatch.StartNew();

                await Task.Run(async () =>
                {
                    averageColors = await calculate(filePaths);
                });

                GC.Collect();
                stopwatch.Stop();

                await new MessageDialog("Done in " + stopwatch.ElapsedMilliseconds + " milliseconds").ShowAsync();

                SelectDestBtn.IsEnabled = true;
            }

        }

        private async Task<IDictionary<Color, List<IImage>>> calculate(IStorageFile[] files)
        {
            ConcurrentDictionary<Color, List<IImage>> images = new ConcurrentDictionary<Color, List<IImage>>();

            IAverageColorCalculator calculator = new AverageColorCalculator();


            var tasks = files.Select(async file =>
            {
                IImage image = new Implementations.Image(file);
                Color average = calculator.CalculateAverage(await image.GetPixels());

                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        progressBar.Value++;
                    });

                List<IImage> imageWithColor;
                if (images.TryGetValue(average, out imageWithColor))
                {
                    imageWithColor.Add(image);
                }
                else
                {
                    images.TryAdd(average, new List<IImage>() { image });
                }

            });

            await Task.WhenAll(tasks);

            return images;
        }

        private async void BtnSelectSourceClick(object sender, RoutedEventArgs e)
        {
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

                int blockSize = 10;

                Stopwatch stopWatch = new Stopwatch();

                progressBar.Maximum = (await image.GetWidth() * await image.GetHeigth()) / (blockSize * blockSize);

                IProgress<int> progress = new Progress<int>(noItems => 
                {
                    progressBar.Value = noItems;
                });

                stopWatch.Start();

                IImageGenerator generator = new ImageGenerator(
                    new ClosestImageSelector(),
                    new AverageColorCalculator(),
                    progress);

                WriteableBitmap newImage = await generator.GenerateImage(image, averageColors, blockSize);

                stopWatch.Stop();

                await new MessageDialog("Done in " + stopWatch.ElapsedMilliseconds + " milliseconds").ShowAsync();

                imageView.Source = newImage;
            }
        }
    }
}
