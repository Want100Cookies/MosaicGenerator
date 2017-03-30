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
using Windows.Storage.Streams;
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
            IAverageColorCalculator calculator = new AverageColorCalculator();
            IPixelReader pixelReader = new PixelReader();

            if (folder != null)
            {
                IStorageFile[] filePaths = await folderReader.ReadFolderAsync(folder);

                Stopwatch stopwatch = Stopwatch.StartNew();

                await Task.Run(() =>
                {
                    calculate(filePaths);    
                });

                GC.Collect();
                stopwatch.Stop();

                await new MessageDialog("Done in " + stopwatch.ElapsedMilliseconds + " milliseconds").ShowAsync();


                //foreach (KeyValuePair<Color, List<IImage>> imageWithColor in images)
                //{
                //    foreach (IImage image in imageWithColor.Value)
                //    {
                //        var panel = new StackPanel()
                //        {
                //            Orientation = Orientation.Horizontal,
                //            HorizontalAlignment = HorizontalAlignment.Center,
                //            VerticalAlignment = VerticalAlignment.Center,
                //            Background = new SolidColorBrush(imageWithColor.Key)
                //        };

                //        panel.Children.Add(new TextBlock()
                //        {
                //            Text = imageWithColor.Key.ToString()
                //        });

                //        //BitmapImage bitmap = new BitmapImage();
                //        //MemoryStream stream = new MemoryStream(image.GetPixels());
                //        //IRandomAccessStream randomStream = stream.AsRandomAccessStream();
                //        //bitmap.SetSource(randomStream);

                //        //panel.Children.Add(new Windows.UI.Xaml.Controls.Image()
                //        //{
                //        //    Source = bitmap as ImageSource,
                //        //    Width = 100,
                //        //    Height = 100,
                //        //    Margin = new Thickness(10, 0, 10, 0)
                //        //});

                //        ImageList.Items.Add(panel);
                //    }
                //}

            }

        }

        private async Task calculate(IStorageFile[] filePaths)
        {
            Dictionary<Color, List<string>> images = new Dictionary<Color, List<string>>();


            var tasks = filePaths.Select(async filePath =>
            {
                using (IRandomAccessStream stream = await filePath.OpenAsync(FileAccessMode.Read))
                {
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

                    PixelDataProvider pixels = await decoder.GetPixelDataAsync(
                        BitmapPixelFormat.Rgba8,
                        BitmapAlphaMode.Straight,
                        new BitmapTransform(),
                        ExifOrientationMode.RespectExifOrientation,
                        ColorManagementMode.ColorManageToSRgb);

                    byte[] colors = pixels.DetachPixelData();

                    int r = 0, g = 0, b = 0;

                    for (int i = 0; i < colors.Length; i += 4)
                    {
                        r += colors[i];
                        g += colors[i + 1];
                        b += colors[i + 2];
                    }

                    float arrayLength = colors.Length / 4;

                    Color average = Color.FromArgb(255, (byte)(r / arrayLength), (byte)(g / arrayLength), (byte)(b / arrayLength));

                    Debug.WriteLine(average.ToString() + " > " + filePath.Name);
                }



                //List<IImage> imageWithColor;
                //if (images.TryGetValue(average, out imageWithColor))
                //{
                //    imageWithColor.Add(image);
                //}
                //else
                //{
                //    images.Add(average, new List<IImage>() { image });
                //}
            });

            await Task.WhenAll(tasks);

        }
    }
}
