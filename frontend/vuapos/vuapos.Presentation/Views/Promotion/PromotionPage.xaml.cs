using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using vuapos.Presentation.Services;
using vuapos.Presentation.ViewModels;
using vuapos.Presentation.Views.Product;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace vuapos.Presentation.Views.Promotion
{
    public sealed partial class PromotionPage : UserControl
    {
        public PromotionViewModel ViewModel { get; }

        public PromotionPage()
        {
            this.InitializeComponent();
            ViewModel = new PromotionViewModel();
            LoadInitialData();
        }

        private async void LoadInitialData()
        {
            await ViewModel.LoadPromotionsAsync();
            CurrentPageTextBlock.Text = $"Page {ViewModel.currentPage} of {ViewModel.totalPages}";

        }
        private async void AddPromotion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               var promotionDialog = new AddPromotionDialog(ViewModel) 
               {
                   XamlRoot = this.XamlRoot
               };
                await promotionDialog.ShowAsync();
                ViewModel.currentPage = 1;
                await ViewModel.LoadPromotionsAsync();
                CurrentPageTextBlock.Text = $"Page {ViewModel.currentPage} of {ViewModel.totalPages}";
            }
            catch (Exception ex)
            {
                await new ContentDialog
                {
                    Title = "Error",
                    Content = $"Failed to open dialog: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }
        }

        private async void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if(ViewModel.currentPage > 1)
            {
                ViewModel.currentPage--;
                CurrentPageTextBlock.Text = $"Page {ViewModel.currentPage} of {ViewModel.totalPages}";
                await ViewModel.LoadPromotionsAsync();
            }
        }

        private async void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.currentPage < ViewModel.totalPages)
            {
                ViewModel.currentPage++;
                CurrentPageTextBlock.Text = $"Page {ViewModel.currentPage} of {ViewModel.totalPages}";

                await ViewModel.LoadPromotionsAsync();
            }

        }

        private async void EditPromotion_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Models.Promotion promotion)
            {
                var editPromotionDialog = new EditPromotionDialog(ViewModel, promotion)
                {
                    XamlRoot = this.XamlRoot
                };
                await editPromotionDialog.ShowAsync();
                await ViewModel.LoadPromotionsAsync();
            }
            else
            {
                await new ContentDialog
                {
                    Title = "Error",
                    Content = "Failed to open dialog.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }
        }
        private async void DeletePromotion_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Models.Promotion promotion)
            {
                var result = await new ContentDialog
                {
                    Title = "Delete Category",
                    Content = $"Are you sure you want to delete {promotion.Name}?",
                    PrimaryButtonText = "Delete",
                    CloseButtonText = "Cancel",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    await ViewModel.DeletePromotionAsync(promotion.Promotion_Id);
                    //await ViewModel.LoadPromotionsAsync();
                    ViewModel.currentPage = 1;
                    await ViewModel.LoadPromotionsAsync();
                    CurrentPageTextBlock.Text = $"Page {ViewModel.currentPage} of {ViewModel.totalPages}";
                    

                }
            }
        }

        private async void PromotionPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadPromotionsAsync();
        }
    }
}
