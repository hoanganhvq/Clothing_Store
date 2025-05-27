using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using vuapos.Presentation.Services;

namespace vuapos.Presentation.Views.Customer
{
    public sealed partial class CustomerDetailDialog : Window
    {
        public Models.Customer Customer { get; }
        public ObservableCollection<Models.Order> CustomerOrders { get; }

        public CustomerDetailDialog(Models.Customer customer, ObservableCollection<Models.Order> customerOrders)
        {
            this.InitializeComponent();
            Customer = customer;
            CustomerOrders = customerOrders;
        }

        private void OnCloseClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private async void OnSearchClicked(object sender, RoutedEventArgs e)
        {
            if (StartDatePicker.SelectedDate is null || EndDatePicker.SelectedDate is null)
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Vui lòng chọn cả ngày bắt đầu và kết thúc.",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                await dialog.ShowAsync();
                return;
            }

            var startDate = StartDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd");
            var endDate = EndDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd");
            var customerId = Customer.Customer_Id;

            try
            {
                CustomerOrders.Clear();
                var service = App.Services!.GetRequiredService<OrderService>();
                var response = await service.GetCustomerOrderByDate(customerId, startDate, endDate);
                if (response == null || response.Data == null)
                {
                    ContentDialog dialog = new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = "Không tìm thấy đơn hàng nào trong khoảng thời gian này.",
                        CloseButtonText = "OK",
                        XamlRoot = this.Content.XamlRoot
                    };
                    await dialog.ShowAsync();
                    return;
                }
                foreach (var order in response.Data)
                {
                    CustomerOrders.Add(order);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi lọc đơn hàng: {ex.Message}");
            }
        }

    }
}
