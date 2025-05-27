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
    public sealed partial class EditPromotionDialog : ContentDialog
    {
        private readonly PromotionViewModel _promotionViewModel;
        private readonly Models.Promotion _promotion;

        public EditPromotionDialog(PromotionViewModel promotionViewModel, Models.Promotion promotion)
        {
            this.InitializeComponent();
            _promotionViewModel = promotionViewModel;
            PromotionNameTextBox.Text= promotion.Name;
            DiscountPercentageTextBox.Text = promotion.Discount_Percentage;
            StartDatePicker.Date = promotion.Start_Date;
            EndDatePicker.Date = promotion.End_Date;
            _promotion = promotion;
        }

        private async void PrimaryButton_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

            try
            {
                ErrorTextBlock.Visibility = Visibility.Collapsed;

                var promotionName = PromotionNameTextBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(promotionName))
                {

                    throw new Exception("Promotion name is required.");
                }

                if (!decimal.TryParse(DiscountPercentageTextBox.Text, out var discountPercentage) || discountPercentage < 0 || discountPercentage > 100)
                {

                    throw new Exception("Discount percentage must be between 0 and 100.");
                }
                Debug.WriteLine(StartDatePicker.Date);
                if (StartDatePicker.Date == null || EndDatePicker.Date == null)
                {

                    throw new Exception("Start date and end date are required.");
                }

                var startDate = StartDatePicker.Date.DateTime;
                var endDate = EndDatePicker.Date.DateTime;
                Debug.WriteLine($"Start date: {startDate}, End date: {endDate}");

                if (startDate > endDate)
                {

                    throw new Exception("End date must be after start date.");
                }

                string start = StartDatePicker.Date.DateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                string end = EndDatePicker.Date.DateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                var promotionCreateDTO = new PromotionUpdateDTO
                {
                    name = promotionName,
                    discount_percentage = discountPercentage,
                    start_date = start,
                    end_date = end,
                };

                await _promotionViewModel.UpdatePromotionAsync(promotionCreateDTO, _promotion.Promotion_Id);

                await _promotionViewModel.LoadPromotionsAsync();

                Hide();
            }
            catch (Exception ex)
            {
                ErrorTextBlock.Text = $"Error: {ex.Message}";
                ErrorTextBlock.Visibility = Visibility.Visible;
                args.Cancel = true;
            };

        }
    }
}
