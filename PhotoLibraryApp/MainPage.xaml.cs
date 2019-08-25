using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml.Shapes;
using System.Diagnostics;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using System.Collections.ObjectModel;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using System.Windows;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PhotoLibraryApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
       
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = Picture.Collection;
        }

        public async void Add_Photos_Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".bmp");

            var files = await picker.PickMultipleFilesAsync();

            try
            {
                await Picture.AddPictures(files);
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message);
                await dialog.ShowAsync();
            }
        }
             

        private void Album_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AlbumPage));
        }

        private void Delete_Photos_Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DeletePhoto));
        }

        private void SelectPhotosButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SelectPhotos));
        }       

        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(PhotoViewPage));
        }

       
    }
}
