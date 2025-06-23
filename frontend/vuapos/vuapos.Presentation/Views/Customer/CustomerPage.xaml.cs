using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using vuapos.Presentation.ViewModels;
using vuapos.Presentation.Models;

namespace vuapos.Presentation.Views.Customer
{
    public sealed partial class CustomerPage : UserControl
    {
        public CustomerViewModel ViewModel { get; } = new();

        public CustomerPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
            LoadCustomers();

        }

        private async void LoadCustomers()
        {
            await ViewModel.LoadCustomersAsync();
        
        }

        private void OnCreateCustomerClicked(object sender, RoutedEventArgs e)
        {
            var newCustomerDialog = new CreateCustomerDialog(ViewModel);
            newCustomerDialog.Activate();
        }

        private async void OnDetailClicked(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var customer = (Models.Customer)button.Tag;
            await ViewModel.getCustomerOrderAsync(customer.Customer_Id);
            var detailDialog = new CustomerDetailDialog(customer, ViewModel.CustomerOrders);
            detailDialog.Activate();
        }

        private void OnEditClicked(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var customer = (Models.Customer)button.Tag;
            var editDialog = new EditCustomerDialog(ViewModel, customer);
            editDialog.Activate();
        }

        private async void OnDeleteClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Models.Customer customer)
            {
                var result = await new ContentDialog
                {
                    Title = "Delete Customer",
                    Content = $"Are you sure you want to delete {customer.Name}?",
                    PrimaryButtonText = "Delete",
                    CloseButtonText = "Cancel",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    await ViewModel.DeleteCustomerAsync(customer);
                }
            }
        }
    }
}
