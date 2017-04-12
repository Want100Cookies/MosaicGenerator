using MosaicGenerator.Abstractions;
using MosaicGenerator.Implementations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace MosaicGenerator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private IDictionary<Color, List<IImage>> averageColors;
        private WriteableBitmap outputBitmap;

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
                SelectDestBtn.IsEnabled = false;
                SaveBtn.IsEnabled = false;

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

                SelectSrcBtn.IsEnabled = true;
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
                SelectSrcBtn.IsEnabled = false;
                SelectDestBtn.IsEnabled = false;
                SaveBtn.IsEnabled = false;

                IImage image = new Implementations.Image(file);

                int blockSize = Int32.Parse(BlocksizeTextbox.Text);

                Stopwatch stopWatch = new Stopwatch();

                int width = await image.GetWidth();
                int height = await image.GetHeight();

                if (width % blockSize != 0)
                {
                    await new MessageDialog("Cannot use specified blocksize. Image width should be divisible by the blocksize!").ShowAsync();
                    SelectSrcBtn.IsEnabled = true;
                    SelectDestBtn.IsEnabled = true;
                    return;
                }

                progressBar.Maximum = (width * height) / (blockSize * blockSize);
                progressBar.Value = 0;

                IProgress<int> progress = new Progress<int>(noItems =>
                {
                    progressBar.Value++;
                });

                stopWatch.Start();

                IClosestImageSelector colorSelector = AdjustSlider.Value > 0 ?
                    (IClosestImageSelector)new AdjustingImageSelector((float)AdjustSlider.Value / 100) :
                    new ClosestImageSelector();

                IImageGenerator generator = new ImageGenerator(
                    colorSelector,
                    new AverageColorCalculator(),
                    progress);

                byte[] newImageBytes = new byte[0];

                await Task.Run(async () =>
                {
                    newImageBytes = await generator.GenerateImage(image, averageColors, blockSize);
                });

                stopWatch.Stop();

                await new MessageDialog("Done in " + stopWatch.ElapsedMilliseconds + " milliseconds").ShowAsync();

                WriteableBitmap newImage = new WriteableBitmap(width, height);
                newImage.FromByteArray(newImageBytes);

                imageView.Source = newImage;
                outputBitmap = newImage;

                SelectSrcBtn.IsEnabled = true;
                SelectDestBtn.IsEnabled = true;
                SaveBtn.IsEnabled = true;
            }
        }

        private async void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker fileSavePicker = new FileSavePicker();
            fileSavePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileSavePicker.FileTypeChoices.Add("JPEG files", new List<string>() { ".jpg" });
            fileSavePicker.SuggestedFileName = "image";

            var outputFile = await fileSavePicker.PickSaveFileAsync();

            if (outputFile == null)
            {
                // The user cancelled the picking operation
                return;
            }
            else
            {
                IImageSaver imageSaver = new ImageSaver();
                await imageSaver.SaveImageAsync(outputBitmap, outputFile);
            }
        }

        private void Slider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            AdjustValueText.Text = $"{(int)AdjustSlider.Value}%";
        }
    }
}