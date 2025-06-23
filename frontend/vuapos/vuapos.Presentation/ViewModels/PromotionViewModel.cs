using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using Microsoft.Extensions.DependencyInjection;
using vuapos.Presentation.DTO.Promotion;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services;

namespace vuapos.Presentation.ViewModels
{
    public class PromotionViewModel
    {
        private readonly PromotionService _promotionService;
        public ObservableCollection<Promotion> Promotions { get; set; } = new();
        public int currentPage { get; set; } = 1;
        public int totalPages { get; set; } = 1;
        public PromotionViewModel()
        {
            _promotionService = App.Services.GetRequiredService<PromotionService>();
        }

        public async Task LoadPromotionsAsync()
        {
            var pagedResponse = await _promotionService.GetPaginationPromotionAsync(currentPage);
            Debug.WriteLine($"Loaded promotions.");
            if (pagedResponse != null)
            {
                totalPages = pagedResponse.TotalPages;

                Promotions.Clear();
                var promotions = pagedResponse.Data;
                foreach (var promotion in promotions)
                    Promotions.Add(promotion);
            }
        }

        public async Task AddPromotionAsync(PromotionCreateDTO promotionCreateDTO)
        {
            
            var response = await _promotionService.AddPromotionAsync(promotionCreateDTO);
            if (response != null)
            {
                Promotions.Add(response);
                await LoadPromotionsAsync();
            }

        }

        public async Task UpdatePromotionAsync(PromotionUpdateDTO promotion, string id)
        {
            var response = await _promotionService.UpdatePromotionAsync(promotion, id);
            if (response != null)
            {
                var index = Promotions.IndexOf(Promotions.FirstOrDefault(x => x.Promotion_Id == id));
                if (index != -1)
                {
                    Promotions[index] = response;
                }
            }
        }
        public async Task DeletePromotionAsync(string id)
        {
            var response = await _promotionService.DeletePromotionAsync(id);

            if (response != null)
            {
                await LoadPromotionsAsync();
            }
        }
    }
}
