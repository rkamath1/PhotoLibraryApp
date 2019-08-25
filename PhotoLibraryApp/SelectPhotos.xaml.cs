using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
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
    public sealed partial class SelectPhotos : Page
    {
        public SelectPhotos()
        {
            this.InitializeComponent();
            this.DataContext = Picture.Collection;
        }

        public async void Add_Photos_Button_ClickAsync(object sender, RoutedEventArgs e)
        {          
            //Create message dialog and set contents
            var confirmation = new MessageDialog("This functionality has not been implemented yet");
            await confirmation.ShowAsync();          
        }

        private async void DeleteItemBtn_Click(object sender, RoutedEventArgs e)
        {
            if(this.SelectGrid.SelectedItems.Count != 0)
            {
                //Create message dialog and set contents
                var confirmation = new MessageDialog("Are you sure you want to delete these photos?");
                //Add commands and set their callbacks
                confirmation.Commands.Add(new UICommand("Yes, Delete Photos", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                confirmation.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                //set command that will be invoked by default & cancel
                confirmation.DefaultCommandIndex = 0;
                confirmation.CancelCommandIndex = 1;
                await confirmation.ShowAsync();
            }
            else
            {
                //Create message dialog and set contents
                var dialog = new MessageDialog("Please select at least one photo");
                await dialog.ShowAsync();
            }           

        }

        private void CommandInvokedHandler(IUICommand command)
        {
            foreach (Picture p in this.SelectGrid.SelectedItems)
            {
                Debug.WriteLine(p.Path);
                Picture.DeletePhotoFromCollection(p.Path);
            }
            Picture.Collection.Clear();
            Picture.LoadAllPicturesAsync();
            this.Frame.Navigate(typeof(MainPage));
        }

        private void CancelSelectionBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        
        private void Collection_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Album_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AlbumPage));
        }
    }
}
