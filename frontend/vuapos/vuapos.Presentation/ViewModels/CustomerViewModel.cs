using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using vuapos.Presentation.Commands;
using vuapos.Presentation.DTO.Customer;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services;
using vuapos.Presentation.ViewModels.vuapos.Presentation.ViewModels.vuapos.Presentation.ViewModels;

namespace vuapos.Presentation.ViewModels
{
    public class CustomerViewModel
    {
        private readonly CustomerService _customerService;
        private Response<Customer> _customerResponse;
        private string _searchText = string.Empty;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                // Implement search logic here if needed
                _ = SearchCustomers();
            }
        }


        public ObservableCollection<Customer> Customers { get; set; } = new();

        public ObservableCollection<Order> CustomerOrders { get; set; } = new ObservableCollection<Order>();

        public PaginationViewModel PaginationViewModel { get; private set; }


        public CustomerViewModel()
        {
            _customerService = App.Services.GetRequiredService<CustomerService>();

            PaginationViewModel = new PaginationViewModel();
            PaginationViewModel.LoadItemsForCurrentPageCommand = new RelayCommand(async _ => await LoadCustomersForCurrentPage());
        }

        public async Task getCustomerOrderAsync(string customerId)
        {
            var customerOrders = await _customerService.GetCustomerOrder(customerId);
            CustomerOrders.Clear();
            if (customerOrders != null)
            {
            
                foreach (var order in customerOrders.Data)
                {
                    CustomerOrders.Add(order);
                }

            } 
        }
        private async Task SearchCustomers()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadCustomersAsync();
                return;
            }
            var searchResponse = await _customerService.SearchCustomersAsync(SearchText);
            if (searchResponse != null)
            {
                Customers.Clear();
                foreach (var customer in searchResponse.Data)
                {
                    Customers.Add(customer);
                }

                PaginationViewModel.TotalItems = searchResponse.TotalCount;
            }
        }

        private async Task LoadCustomersForCurrentPage()
        {
            var respponse = await _customerService.GetAllCustomersAsync(PaginationViewModel.CurrentPage);
            if (respponse != null)
            {
                Customers.Clear();
                foreach (var customer in respponse.Data)
                {
                    Customers.Add(customer);
                }
            }
            else
            {
                Customers.Clear();
            }
        }

        // Load customers from API
        public async Task LoadCustomersAsync()
        {
            _customerResponse = await _customerService.GetAllCustomersAsync(1);
            var customers = _customerResponse.Data;
            if (customers != null)
            {
                Customers.Clear();
                foreach (var customer in customers)
                    Customers.Add(customer);
            }
            PaginationViewModel.TotalItems = _customerResponse.TotalCount;
        }

        // Add a new customer
        public async Task AddCustomerAsync(string name, string phone, string email)
        {
            var newCustomer = new CustomerCreateDTO
            {
                name = name,
                phone = phone,
                email = email
            };
            var addedCustomer = await _customerService.CreateCustomerAsync(newCustomer);

            if (addedCustomer != null) _ = LoadCustomersAsync();
        }

        // Update existing customer
        public async Task UpdateCustomerAsync(Customer customer, CustomerUpdateDTO updateDto)
        {
            var updatedCustomer = await _customerService.UpdateCustomerAsync(customer.Customer_Id, updateDto);

            if (updatedCustomer != null)
            {
                // Update customer in the list
                await LoadCustomersAsync();
            }
        }

        // Delete customer
        public async Task DeleteCustomerAsync(Customer customer)
        {
            bool success = await _customerService.DeleteCustomerAsync(customer.Customer_Id);
            if (success)
            {
                // Remove customer from the list
                _ = LoadCustomersAsync();
            }
        }

        // Fetch customer details
        public async Task<Customer?> GetCustomerByIdAsync(string customerId)
        {
            return await _customerService.GetCustomerByIdAsync(customerId);
        }
    }
}
