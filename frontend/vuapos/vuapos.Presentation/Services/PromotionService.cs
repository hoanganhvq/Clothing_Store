using Microsoft.Extensions.DependencyInjection;
//using System.Text.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using vuapos.Presentation.DTO.Promotion;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services.Interfaces;
using vuapos.Presentation.Views.Category;

namespace vuapos.Presentation.Services
{
    public class PromotionService: ApiService
    {
        public PromotionService(HttpClient httpClient): base(httpClient)
        {
            base.Token = App.Services!.GetRequiredService<IUserSession>().Token;
        }
        public async Task<PagePromotionResponse<Promotion>?> GetPaginationPromotionAsync(int page = 1)
        {

            return await SendRequestAsync<PagePromotionResponse<Promotion>>(HttpMethod.Get, $"promotions?page={page}");

        }
        public async Task<Promotion?> AddPromotionAsync(PromotionCreateDTO promotionCreateDTO)
        {
            string json = JsonConvert.SerializeObject(promotionCreateDTO);
            Debug.WriteLine("➡️ JSON gửi lên server:");
            Debug.WriteLine(json);

            Debug.WriteLine(promotionCreateDTO.name);

            Debug.WriteLine("duweuhfwhefoihweofhoewhf");


            return await SendRequestAsync<Promotion>(HttpMethod.Post, "promotions", promotionCreateDTO);
        }

        public async Task<Promotion?> UpdatePromotionAsync(PromotionUpdateDTO promotion, int id)
        {

            string json = JsonConvert.SerializeObject(promotion);
            Debug.WriteLine("➡️ JSON gửi lên server:");
            Debug.WriteLine(json);

            return await SendRequestAsync<Promotion>(HttpMethod.Patch, $"promotions/{id}", promotion);
        }
        public async Task<Promotion?> DeletePromotionAsync(int id)
        {
            return await SendRequestAsync<Promotion>(HttpMethod.Delete, $"promotions/{id}");
        }

    }
}
