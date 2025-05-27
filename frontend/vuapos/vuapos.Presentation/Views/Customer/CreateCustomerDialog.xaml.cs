using Microsoft.UI.Xaml;
using vuapos.Presentation.ViewModels;

namespace vuapos.Presentation.Views.Customer
{
    public sealed partial class CreateCustomerDialog : Window
    {
        private readonly CustomerViewModel _viewModel;

        public CreateCustomerDialog(CustomerViewModel viewModel)
        {
            this.InitializeComponent();
            _viewModel = viewModel;
        }

        private async void OnCreateClicked(object sender, RoutedEventArgs e)
        {
            await _viewModel.AddCustomerAsync(NameInput.Text, PhoneInput.Text, EmailInput.Text);
            this.Close();
        }
    }
}
