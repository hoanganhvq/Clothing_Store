using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using vuapos.Presentation.Services;
using vuapos.Presentation.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace vuapos.Presentation.Views.FrequentlyBoughtTogether
{
    public sealed partial class FrequentlyBoughtTogether : UserControl
    {
        bool _hasLoaded = false;
        public FrequentlyBoughtTogetherViewModel ViewModel { get; } = new();

        public FrequentlyBoughtTogether()
        {
            this.InitializeComponent();
            
            ViewModel = new FrequentlyBoughtTogetherViewModel();
            //FrequentlyBoughtTogether_Loaded(_hasLoaded);
            this.Loaded += FrequentlyBoughtTogether_Loaded;

        }

        private async void FreqPage_Loaded(object sender, RoutedEventArgs e)
        {
            _hasLoaded = true;

            //await ViewModel.LoadFrequentlyBoughtTogetherAsync();
            try
            {
                bool isLoaded = await ViewModel.LoadFrequentlyBoughtTogetherAsync();
                Debug.WriteLine(isLoaded);
                if (!isLoaded)
                {
                    Debug.WriteLine("test");
                    await ShowNoGroupsDialog();
                }
                //await ViewModel.LoadFrequentlyBoughtTogetherAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading frequently bought together: {ex.Message}");
                await ShowNoGroupsDialog();
            }
        }

        private async void FrequentlyBoughtTogether_Loaded(object sender, RoutedEventArgs e)
        {
            if (_hasLoaded) return;
            _hasLoaded = true;

            try
            {
                bool isLoaded = await ViewModel.LoadFrequentlyBoughtTogetherAsync();
                Debug.WriteLine(isLoaded);
                if (!isLoaded)
                {
                    Debug.WriteLine("test");
                    await ShowNoGroupsDialog();
                }
                //await ViewModel.LoadFrequentlyBoughtTogetherAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading frequently bought together: {ex.Message}");
                await ShowNoGroupsDialog();
            }


        }
        private async Task ShowNoGroupsDialog()
        {
            Debug.WriteLine("testt");
            ContentDialog noGroupsDialog = new ContentDialog
            {
                Title = "Notification",
                Content = "No product group found.",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await noGroupsDialog.ShowAsync();
        }
    }
}
