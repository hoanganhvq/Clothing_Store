using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.DTO.OrderDetail;
using vuapos.Presentation.Services.Interfaces;
    using Models = vuapos.Presentation.Models;
using vuapos.Presentation.Views.FrequentlyBoughtTogether;

namespace vuapos.Presentation.Services
{
    public class FrequentlyBoughtTogetherService: ApiService
    {
        public FrequentlyBoughtTogetherService(HttpClient httpClient) : base(httpClient)
        {
            // base.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdGFmZl9pZCI6IjhmOWUwNmUxLTM1ZWQtNDViYy05M2Y2LWExN2YyZGIyNmMzOSIsInJvbGUiOiJNQU5BR0VSIiwiaWF0IjoxNzQ1NjYxODA5LCJleHAiOjE3NDYyNjY2MDl9.3Myou0ILU61jkT4B0Xv75qrQA7qGWBOegBCREpjnEoI";
            base.Token = App.Services!.GetRequiredService<IUserSession>().Token;
        }
        public async Task<FrequentlyBoughtTogether?> GetFrequentlyBoughtTogetherAsync(List<Models.Product> products, List<OrderDetailResponse> orderItems)
        {

            if (products == null || !products.Any())
            {
                Debug.WriteLine("No products provided for analysis.");
                return null;
            }
            if (orderItems == null || !orderItems.Any())
            {
                Debug.WriteLine("No order items provided for analysis.");
                return null;
            }
            foreach (var p in products)
            {
                p.Image_Path ??= ""; // nếu null thì gán chuỗi rỗng
                p.Category_Name ??= ""; // phòng trường hợp lỗi tiếp theo
            }

            var requestData = new
            {
                products = products,               // đúng tên
                order_details = orderItems,        // đúng tên
                min_support = 0.1,
                min_confidence = 0.5
            };

            Debug.WriteLine("Sending request to /analyze");
            return await SendRequestAsync<FrequentlyBoughtTogether>(
                HttpMethod.Post,
                "http://localhost:8000/analyze/frequent-products",
                requestData
            );
        }
       
    }
}


