using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PhotoLibraryApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateAlbum : Page
    {
        public CreateAlbum()
        {
            this.InitializeComponent();
        }
        private void Collection_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        private void Album_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AlbumPage));
        }

        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            string textFile = "";
            //name of file to store album info
            if (albumName.Text != "")
            {
                textFile = albumName.Text + ".txt";

                var picker = new Windows.Storage.Pickers.FileOpenPicker();
                picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");
                picker.FileTypeFilter.Add(".png");
                picker.FileTypeFilter.Add(".bmp");

                var files = await picker.PickMultipleFilesAsync();

                try
                {
                    //Add pictures to album file
                    await AlbumHelper.AddPictures(files, textFile);
                }

                catch (Exception ex)
                {
                    var dialog = new MessageDialog(ex.Message);
                    await dialog.ShowAsync();
                }

                this.Frame.Navigate(typeof(AlbumPage));
            }
            else
            {
                var dialog = new MessageDialog("Please enter an album name");
                await dialog.ShowAsync();
            }


        }
    }
}
