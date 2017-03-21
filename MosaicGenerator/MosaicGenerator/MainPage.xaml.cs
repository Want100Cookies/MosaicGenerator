using MosaicGenerator.Abstractions;
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
using Windows.UI.Xaml.Media.Imaging;
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
            IFolderReader folderReader = new FolderReader();
            StorageFolder folder = await folderReader.PickFolderAsync();


            if (folder != null)
            {
                IStorageFile[] filePaths = await folderReader.ReadFolderAsync(folder);

                IImageReader imageReader = new ImageReader();
                IPixelReader pixelReader = new PixelReader();
                IAverageColorCalculator calculator = new AverageColorCalculator(pixelReader);

                Dictionary<Color, List<SoftwareBitmap>> images = new Dictionary<Color, List<SoftwareBitmap>>();

                Stopwatch stopwatch = Stopwatch.StartNew();

                var tasks = filePaths.Select(async filePath =>
                {
                    SoftwareBitmap image = await imageReader.ReadImageAsync(filePath);
                    Color average = await calculator.CalculateAverage(image);

                    List<SoftwareBitmap> imageWithColor;
                    if (images.TryGetValue(average, out imageWithColor))
                    {
                        imageWithColor.Add(image);
                    }
                    else
                    {
                        images.Add(average, new List<SoftwareBitmap>() { image });
                    }
                });

                await Task.WhenAll(tasks);

                stopwatch.Stop();

                await new MessageDialog("Done in " + stopwatch.ElapsedMilliseconds + " milliseconds").ShowAsync();


                foreach (KeyValuePair<Color, List<SoftwareBitmap>> imageWithColor in images)
                {
                    foreach (SoftwareBitmap image in imageWithColor.Value)
                    {
                        var panel = new StackPanel()
                        {
                            Orientation = Orientation.Horizontal,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                        };

                        panel.Children.Add(new TextBlock()
                        {
                            Text = imageWithColor.Key.ToString()
                        });

                        var source = new SoftwareBitmapSource();
                        await source.SetBitmapAsync(SoftwareBitmap.Convert(image, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied));

                        panel.Children.Add(new Image()
                        {
                            Source = source,
                            Width = 100,
                            Height = 100,
                            Margin = new Thickness(10, 0, 10, 0)
                        });

                        ImageList.Items.Add(panel);
                    }
                }

            }

        }
    }
}
