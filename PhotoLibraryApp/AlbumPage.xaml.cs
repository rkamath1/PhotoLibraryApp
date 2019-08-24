using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PhotoLibraryApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AlbumPage : Page
    {
        public AlbumPage()
        {
            this.InitializeComponent();
        }

        private async Task LoadPicturesFromAlbums()
        {
            //Ordering album files by date
            // initialize queryOptions using a common query
            QueryOptions queryOptions = new QueryOptions(CommonFileQuery.DefaultQuery, new string[] { ".txt" });

            // clear all existing sorts
            queryOptions.SortOrder.Clear();

            // add ascending sort by date modified
            SortEntry se = new SortEntry();
            se.PropertyName = "System.DateModified";
            se.AscendingOrder = true;
            queryOptions.SortOrder.Add(se);

            //Open folder containing album files
            var storageFolder = ApplicationData.Current.LocalFolder;

            //Use query to sort album data text files by date
            StorageFileQueryResult queryResult = storageFolder.CreateFileQueryWithOptions(queryOptions);
            IReadOnlyList<StorageFile> textFiles = await queryResult.GetFilesAsync();

            foreach (var textFile in textFiles)
            {
                if (textFile.Name != "Library.txt")
                {
                    await dynamicControlAsync(textFile.Name);
                }
            }
        }

        private async Task dynamicControlAsync(string textFileName)
        {
            //Add flipview dynamically                                         
            // Create a new flip view, add content

            FlipView flipView1 = new FlipView
            {
                Width = 150,
                Height = 150,
                BorderThickness = new Thickness(4)                
            };

            //Capture the album name
            TextBlock textBlock1 = new TextBlock();
            string[] split = textFileName.Split('.');
            textBlock1.Text = split[0];

            StackPanel stackPanel = new StackPanel();

            // Data source  
            //Get photos from Albums
            string content = await FileHelper.ReadTextFileAsync(textFileName);

            if (!string.IsNullOrWhiteSpace(content))
            {
                var fileList = content.Split('\n', '\r');

                foreach (string filePath in fileList)
                {
                    if (string.IsNullOrEmpty(filePath))
                    {
                        continue;
                    }

                    var storageFile = await StorageFile.GetFileFromPathAsync(filePath);
                    var bitmapImage = new BitmapImage();
                    Image image = new Image();

                    using (StorageItemThumbnail thumb = await storageFile.GetThumbnailAsync(ThumbnailMode.SingleItem))
                    {
                        bitmapImage.SetSource(thumb);
                    }
                    image.Source = bitmapImage;
                    flipView1.Items.Add(image);
                }
            }

            // Add the flip view to a parent container in the visual tree.
            stackPanel.Children.Add(textBlock1);
            stackPanel.Children.Add(flipView1);
            stack1.Children.Add(stackPanel);
        }


        private void Add_Album_Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateAlbum));
        }


        private void Collection_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadPicturesFromAlbums();
        }
    }
}
