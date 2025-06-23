using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using vuapos.Presentation.ViewModels;
using vuapos.Presentation.Views.OrderDetail;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace vuapos.Presentation.Views.Order
{
    public sealed partial class OrderPage : UserControl
    {
        public OrderViewModel ViewModel { get; set; }
        public OrderPage()
        {
            this.InitializeComponent();

            ViewModel = App.Services!.GetRequiredService<OrderViewModel>();
            this.DataContext = ViewModel;
            //
            this.Loaded += OrderPage_Loaded; ;

        }

        private void OrderPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.XamlRoot != null)
            {
                ViewModel.UpdateXamlRoot(this.XamlRoot);
            }
        }

        private void ViewOrder_Click(object sender, RoutedEventArgs e)
        {  
            if (sender is Button button && button.Tag is Models.Order order)
            {

                ViewModel.SelectedOrder = order;
                OrderDetailPage orderDetailPage = new OrderDetailPage(ViewModel);
                orderDetailPage.Activate();
            }
        }

        private void ApplyDateFilter_Click(object sender, RoutedEventArgs e)
        {
            if (StartDatePicker.SelectedDate is null || EndDatePicker.SelectedDate is null)
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Please select both start and end dates.",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                _ = dialog.ShowAsync();
                return;
            }
            var startDate = StartDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd");
            var endDate = EndDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd");
            _ = ViewModel.LoadOrdersByDate(startDate, endDate);
        }

        private void DateFilter_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            if (StartDatePicker.SelectedDate is not null && EndDatePicker.SelectedDate is not null)
            {
             
            }
        }

        private void ClearDateFilter_Click(object sender, RoutedEventArgs e)
        {
            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;
            _ = ViewModel.LoadOrders();
        }
        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedOrder = null;
            OrderDetailPage orderDetailPage = new OrderDetailPage(ViewModel);
            orderDetailPage.Activate();
        }


    }
}
