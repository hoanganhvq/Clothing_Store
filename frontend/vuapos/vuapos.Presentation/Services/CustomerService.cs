using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using vuapos.Presentation.DTO.Customer;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services.Interfaces;

namespace vuapos.Presentation.Services
{
    public class CustomerService : ApiService
    {
      
        public CustomerService(HttpClient httpClient) : base(httpClient)
        {
            //base.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdGFmZl9pZCI6IjhmOWUwNmUxLTM1ZWQtNDViYy05M2Y2LWExN2YyZGIyNmMzOSIsInJvbGUiOiJNQU5BR0VSIiwiaWF0IjoxNzQ1NjYxODA5LCJleHAiOjE3NDYyNjY2MDl9.3Myou0ILU61jkT4B0Xv75qrQA7qGWBOegBCREpjnEoI";
            base.Token = App.Services!.GetRequiredService<IUserSession>().Token;
        }

        public async Task<Response<Customer>?> GetAllCustomersAsync(int page)
        {
            return await SendRequestAsync<Response<Customer>>(HttpMethod.Get, $"customer?page={page}");
        }

        public async Task<Response<Order>?> GetCustomerOrder(string id)
        {
            return await SendRequestAsync<Response<Order>>(HttpMethod.Get, $"order?search={id}");
        }

        public async Task<Response<Customer>?> SearchCustomersAsync(string name)
        {
            return await SendRequestAsync<Response<Customer>>(HttpMethod.Get, $"customer?search={name}");
        }

        public async Task<Customer?> GetCustomerByIdAsync(string customerId)
        {
            return await SendRequestAsync<Customer>(HttpMethod.Get, $"customer/{customerId}");
        }

        public async Task<Customer?> CreateCustomerAsync(CustomerCreateDTO customer)
        {
            return await SendRequestAsync<Customer>(HttpMethod.Post, "customer", customer);
        }

        public async Task<Customer?> UpdateCustomerAsync(string customerId, object updateData)
        {
            return await SendRequestAsync<Customer>(HttpMethod.Patch, $"customer/{customerId}", updateData);
        }

        public async Task<bool> DeleteCustomerAsync(string customerId)
        {
            var response = await SendRequestAsync<object>(HttpMethod.Delete, $"customer/{customerId}");
            return response != null;
        }
    }
}
