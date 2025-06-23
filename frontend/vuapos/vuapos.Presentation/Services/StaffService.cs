using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using vuapos.Presentation.DTO.Customer;
using vuapos.Presentation.DTO.Staff;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services.Interfaces;
using vuapos.Presentation.Views.Customer;

namespace vuapos.Presentation.Services
{
    public class StaffService : ApiService
    {

        public StaffService(HttpClient httpClient) : base(httpClient)
        {
            base.Token = App.Services!.GetRequiredService<IUserSession>().Token;
        }

        public async Task<Response<Staff>> GetAllStaffsAsync(int page)
        {
            return await SendRequestAsync<Response<Staff>>(HttpMethod.Get, $"staff?page={page}");
        }

        public async Task<Response<Staff>> GetStaffsByNameAsync(string name)
        {
            return await SendRequestAsync<Response<Staff>>(HttpMethod.Get, $"staff?search={name}");
        }


        public async Task<Staff?> GetStaffByIdAsync(string id)
        {
            return await SendRequestAsync<Staff>(HttpMethod.Get, $"staff/{id}");
        }
        public async Task<bool> CreateStaffAsync(StaffDTO staff)
        {
            var response = await SendRequestAsync<Staff>(HttpMethod.Post, "staff", staff);
            return response != null;
        }
        public async Task<bool> UpdateStaffAsync(string staffId, StaffDTO updateData)
        {
            var response = await SendRequestAsync<Staff>(HttpMethod.Patch, $"staff/{staffId}", updateData);
            return response != null;
        }

        public async Task<bool> DeleteStaffAsync(string staffId)
        {
            var response = await SendRequestAsync<object>(HttpMethod.Delete, $"staff/{staffId}");
            return response != null;
        }


    }
}
