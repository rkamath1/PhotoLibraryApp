using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.UI.Popups;

namespace PhotoLibraryApp
{
    public class Picture
    {
        // File to store application's data
        private const string TEXT_FILE_NAME = "Library.txt";

        // Global collection of pictures
        public static ObservableCollection<Picture> Collection = new ObservableCollection<Picture>();

        // Path of the picture file
        public string Path { get; set; }

        // BitmapImage to be used as the souce of the image control
        public BitmapImage ImageSource { get; set; }

        /// <summary>
        /// Adds pictures to the library and updates storage file
        /// </summary>
        /// <param name="picture"></param>
        public static async Task AddPictures(IReadOnlyList<StorageFile> storageFiles)
        {
            foreach (var storageFile in storageFiles)
            {

                if (Collection.Any(p => p.Path == storageFile.Path) == false)
                {
                    // Add picture to the global collection
                    await AddPictureToCollection(storageFile.Path);

                    // Save picture file path in storage data file
                    FileHelper.WriteTextFileAsync(TEXT_FILE_NAME, storageFile.Path);
                }
                else
                {
                    // show mwssage to the user
                    var dialog = new MessageDialog($"The file '{storageFile.Path}' already exists in the collection.");
                    await dialog.ShowAsync();
                }
            }
        }

        private static async Task AddPictureToCollection(string filePath)
        {
            // Create a bitmap
            StorageFile storageFile = null;

            try
            {
                storageFile = await StorageFile.GetFileFromPathAsync(filePath);
            }
            catch (UnauthorizedAccessException)
            {
                throw new Exception("Access denied to the folder");
            }

            BitmapImage bitmapImage = new BitmapImage();
            using (var stream = await storageFile.OpenAsync(FileAccessMode.Read))
            {
                bitmapImage.SetSource(stream);
            }

            // Create Picture object
            var pic = new Picture();           
            pic.Path = storageFile.Path;
            pic.ImageSource = bitmapImage;

            // Add Picture object to the global observable collection
            Collection.Add(pic);
        }
        /// <summary>
        /// Loads all pictures from the library data file
        /// </summary>
        /// <returns>A list of pictures</returns>
        public async static Task LoadAllPicturesAsync()
        {
            var content = await FileHelper.ReadTextFileAsync(TEXT_FILE_NAME);
            char[] charSeparators = new char[] { '\n', '\r' };

            if (!string.IsNullOrWhiteSpace(content))
            {
                var fileList = content.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);

                foreach (var file in fileList)
                {
                    await ShowPicturesAsync(file);
                }
            }
        }

        private static async Task ShowPicturesAsync(string filePath)
        {
            // Create a bitmap
            StorageFile storageFile = await StorageFile.GetFileFromPathAsync(filePath);           
            BitmapImage bitmapImage = new BitmapImage();

            //using (var stream = await storageFile.OpenAsync(FileAccessMode.Read))
            //{
            //    bitmapImage.SetSource(stream);
            //}

            using (StorageItemThumbnail thumbNail = await storageFile.GetThumbnailAsync(ThumbnailMode.SingleItem))
            {
                bitmapImage.SetSource(thumbNail);
            }

            // Create Picture object
            var pic = new Picture
            {
                Path = storageFile.Path,
                ImageSource = bitmapImage
            };

            // Add Picture object to the global observable collection
            Collection.Add(pic);
        }

    }
    
}