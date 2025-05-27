using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
//using System.Text.Json;
using Newtonsoft.Json;

using System.Text.Json.Serialization;
using System.Threading.Tasks;
using vuapos.Presentation.DTO.Promotion;
using vuapos.Presentation.Views.Category;
using vuapos.Presentation.Models;

namespace vuapos.Presentation.Services
{
    public class PromotionService: ApiService
    {
        public PromotionService(HttpClient httpClient): base(httpClient)
        {
            base.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdGFmZl9pZCI6IjhmOWUwNmUxLTM1ZWQtNDViYy05M2Y2LWExN2YyZGIyNmMzOSIsInJvbGUiOiJNQU5BR0VSIiwiaWF0IjoxNzQ1NjYxODA5LCJleHAiOjE3NDYyNjY2MDl9.3Myou0ILU61jkT4B0Xv75qrQA7qGWBOegBCREpjnEoI";
        }
        public async Task<PagePromotionResponse<Promotion>?> GetPaginationPromotionAsync(int page = 1)
        {
            return await SendRequestAsync<PagePromotionResponse<Promotion>>(HttpMethod.Get, $"promotions?page={page}");

        }
        public async Task<Promotion?> AddPromotionAsync(PromotionCreateDTO promotionCreateDTO)
        {
            Debug.WriteLine(promotionCreateDTO.name);

            Debug.WriteLine("duweuhfwhefoihweofhoewhf");


            return await SendRequestAsync<Promotion>(HttpMethod.Post, "promotions", promotionCreateDTO);
        }

        public async Task<Promotion?> UpdatePromotionAsync(PromotionUpdateDTO promotion, string id)
        {
            return await SendRequestAsync<Promotion>(HttpMethod.Patch, $"promotions/{id}", promotion);
        }
        public async Task<Promotion?> DeletePromotionAsync(string id)
        {
            return await SendRequestAsync<Promotion>(HttpMethod.Delete, $"promotions/{id}");
        }

    }
}
