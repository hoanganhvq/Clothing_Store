using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services.Interfaces;
using vuapos.Presentation.Views.Product;

namespace vuapos.Presentation.Services
{
    public interface ICategoryService
    {
        Task<List<Category>?> GetAllCategoriesAsync();
        Task<PageCategoryResponse<Category>?> GetPaginationCategoriesAsync(int page = 1);

        Task<Category?> AddCategoryAsync(string name);
        Task<Category?> UpdateCategoryAsync(string customer_id, string name);
        Task<Category?> DeleteCategoryAsync(string customer_id);
    }
    public class CategoryService : ApiService, ICategoryService
    {
        public CategoryService(HttpClient httpClient) : base(httpClient)
        {
            //base.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdGFmZl9pZCI6IjhmOWUwNmUxLTM1ZWQtNDViYy05M2Y2LWExN2YyZGIyNmMzOSIsInJvbGUiOiJNQU5BR0VSIiwiaWF0IjoxNzQ1NjYxODA5LCJleHAiOjE3NDYyNjY2MDl9.3Myou0ILU61jkT4B0Xv75qrQA7qGWBOegBCREpjnEoI";
            base.Token = App.Services!.GetRequiredService<IUserSession>().Token;
        }

        public async Task<PageCategoryResponse<Category>?> GetPaginationCategoriesAsync(int page = 1)
        {
            return await SendRequestAsync<PageCategoryResponse<Category>>(HttpMethod.Get, $"category?page={page}");

        }
        public async Task<List<Category>?> GetAllCategoriesAsync()
        {

            return await SendRequestAsync<List<Category>>(HttpMethod.Get, "category?return-all=true");
        }
        public async Task<Category?> AddCategoryAsync(string name)
        {
            var categoryData = new { name };
            return await SendRequestAsync<Category>(HttpMethod.Post, "category", categoryData);
        }

        public async Task<Category?> UpdateCategoryAsync(string customer_id, string name)
        {
            var categoryData = new { name };
            return await SendRequestAsync<Category>(HttpMethod.Patch, $"category/{customer_id}", categoryData);
        }

        public async Task<Category?> DeleteCategoryAsync(string customer_id)
        {
            return await SendRequestAsync<Category>(HttpMethod.Delete, $"category/{customer_id}");
        }
    }
}
