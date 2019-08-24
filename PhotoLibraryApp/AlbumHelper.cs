using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace PhotoLibraryApp
{
    class AlbumHelper
    {
        // Global collection of pictures
        public static ObservableCollection<AlbumHelper> Album = new ObservableCollection<AlbumHelper>();
        //public static Collection<ObservableCollection<AlbumPic>> Albums = new Collection<ObservableCollection<AlbumPic>>();

        // Path of the picture file
        public string Path1 { get; set; }

        // BitmapImage to be used as the souce of the image control
        public BitmapImage ImageSource1 { get; set; }

        // Album name
        public string AlbumName { get; set; }

        /// <summary>
        /// Adds pictures to an albumy and updates storage file
        /// </summary>
        /// <param name="picture"></param>
        public static async Task AddPictures(IReadOnlyList<StorageFile> storageFiles, string textFileName)
        {
            foreach (var storageFile in storageFiles)
            {
                // Add picture to the global collection
                await AddPictureToAlbum(storageFile.Path, textFileName);

                // Save picture file path in storage data file
                FileHelper.WriteTextFileAsync(textFileName, storageFile.Path);
            }
        }

        private static async Task AddPictureToAlbum(string filePath, string textFileName)
        {
            char[] charSeparators = new char[] { '.' };
            var name = textFileName.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
            StorageFile storageFile = null;

            try
            {
                storageFile = await StorageFile.GetFileFromPathAsync(filePath);
            }
            catch (UnauthorizedAccessException)
            {
                throw new Exception("Access denied to the folder");
            }

            // Create a bitmap
            var bitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
            using (var stream = await storageFile.OpenAsync(FileAccessMode.Read))
            {
                bitmapImage.SetSource(stream);
            }

            //ObservableCollection<AlbumPic> Album = new ObservableCollection<AlbumPic>();

            // Create Picture object
            var pic = new AlbumHelper();
            pic.Path1 = storageFile.Path;
            pic.ImageSource1 = bitmapImage;
            pic.AlbumName = name[0];

            //Add picture to album
            Album.Add(pic);

            // Add Album object to the global observable collection
            // Albums.Add(Album);
        }
        public async static Task LoadPicturesFromAlbums()
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            IReadOnlyList<StorageFile> textFiles = await storageFolder.GetFilesAsync();

            foreach (var textFile in textFiles)
            {
                if (textFile.Name != "library.txt")
                {
                    await LoadAllPicturesAsync(textFile.Name);
                }
            }
        }

        public async static Task LoadAllPicturesAsync(string textFileName)
        {

            var content = await FileHelper.ReadTextFileAsync(textFileName);

            if (!string.IsNullOrWhiteSpace(content))
            {
                var fileList = content.Split();

                foreach (string filePath in fileList)
                {
                    if (string.IsNullOrEmpty(filePath))
                    {
                        continue;
                    }

                    await AddPictureToAlbum(filePath, textFileName);
                }
            }
        }
    }
}
