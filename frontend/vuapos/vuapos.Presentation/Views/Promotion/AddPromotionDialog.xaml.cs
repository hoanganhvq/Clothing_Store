using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using vuapos.Presentation.DTO.Promotion;
using vuapos.Presentation.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace vuapos.Presentation.Views.Promotion
{
    public sealed partial class AddPromotionDialog : ContentDialog
    {
        private readonly PromotionViewModel _promotionViewModel;
        public AddPromotionDialog(PromotionViewModel promotionViewModel)
        {
            this.InitializeComponent();
            _promotionViewModel = promotionViewModel;
        }

        private async void PrimaryButton_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //var deferral = args.GetDeferral();

            try
            {
                ErrorTextBlock.Visibility = Visibility.Collapsed;

                var promotionName = PromotionNameTextBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(promotionName))
                {
                    ErrorTextBlock.Text = "Promotion name is required.";
                    ErrorTextBlock.Visibility = Visibility.Visible;
                    args.Cancel = true;
                    return;
                }

                if (!decimal.TryParse(DiscountPercentageTextBox.Text, out var discountPercentage) || discountPercentage < 0 || discountPercentage > 100)
                {
                    ErrorTextBlock.Text = "Discount percentage must be between 0 and 100.";
                    ErrorTextBlock.Visibility = Visibility.Visible;
                    args.Cancel = true;
                    return;
                }
                Debug.WriteLine(StartDatePicker.Date);
                if (StartDatePicker.Date == null || EndDatePicker.Date == null)
                {
                    ErrorTextBlock.Text = "Start date and end date are required.";
                    ErrorTextBlock.Visibility = Visibility.Visible;
                    args.Cancel = true;
                    return;
                }

                var startDate = StartDatePicker.Date.ToString();
                var endDate = EndDatePicker.Date.ToString();

                
                //if (startDate > endDate)
                //{
                //    ErrorTextBlock.Text = "End date must be after start date.";
                //    ErrorTextBlock.Visibility = Visibility.Visible;
                //    args.Cancel = true;
                //    deferral.Complete();
                //    return;
                //}

                string start = StartDatePicker.Date.DateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                string end = EndDatePicker.Date.DateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                var promotionCreateDTO = new PromotionCreateDTO
                {
                    name = promotionName,
                    discount_percentage = discountPercentage,
                    start_date = start,
                    end_date = end,
                };

                await _promotionViewModel.AddPromotionAsync(promotionCreateDTO);
                
                await _promotionViewModel.LoadPromotionsAsync();
               
                //else
                //{
                //    ErrorTextBlock.Text = "Failed to add promotion.";
                //    ErrorTextBlock.Visibility = Visibility.Visible;
                //    args.Cancel = true;
                //}
            }
            catch (Exception ex)
            {
                ErrorTextBlock.Text = $"Error: {ex.Message}";
                ErrorTextBlock.Visibility = Visibility.Visible;
                args.Cancel = true;
            }
        }
    }
}