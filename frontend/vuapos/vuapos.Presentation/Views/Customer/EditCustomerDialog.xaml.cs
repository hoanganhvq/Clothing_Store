using Microsoft.UI.Xaml;
using vuapos.Presentation.DTO.Customer;
using vuapos.Presentation.ViewModels;

namespace vuapos.Presentation.Views.Customer
{
    public sealed partial class EditCustomerDialog : Window
    {
        private readonly CustomerViewModel _viewModel;
        public Models.Customer Customer { get; }

        public EditCustomerDialog(CustomerViewModel viewModel, Models.Customer customer)
        {
            this.InitializeComponent();
            _viewModel = viewModel;
            Customer = customer;
        }

        private async void OnSaveClicked(object sender, RoutedEventArgs e)
        {
            var updatedCustomerDto = new CustomerUpdateDTO
            {
                name = Customer.Name,
                phone = Customer.Phone,
                email = Customer.Email,
                point = Customer.Point
            };

            await _viewModel.UpdateCustomerAsync(Customer, updatedCustomerDto);
            this.Close();
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
