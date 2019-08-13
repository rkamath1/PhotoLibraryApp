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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PhotoLibraryApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<string> current;
        public MainPage()
        {
            this.InitializeComponent();
            current = new ObservableCollection<string>();
            current.Add("C://Users/Ranja/Pictures/2017-08/IMG_4138.JPG");
            current.Add("C://Users/Ranja/Pictures/2017-08/IMG_4139.JPG");
            current.Add("C://Users/Ranja/Pictures/2017-08/IMG_4140.JPG");
            this.DataContext = current;
        }

        private async void Import_Photos_Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".jpeg");
                openPicker.FileTypeFilter.Add(".png");

                StorageFile file = await openPicker.PickSingleFileAsync();


                if (file != null)
                {
                    // Application now has read/write access to the picked file
                    this.textBlock.Text = "Picked photo: " + file.Name;

                }
                else
                {
                    this.textBlock.Text = "Operation cancelled.";
                }

            }


        }

        private void Album_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AlbumPage));
        }
    }
}
