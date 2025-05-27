using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.DependencyInjection;
using vuapos.Presentation.DTO.Order;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services.Interfaces;
using vuapos.Presentation.Views.Customer;

namespace vuapos.Presentation.Services
{
    public class OrderService : ApiService
    {
        public OrderService(HttpClient httpClient) : base(httpClient)
        {
            base.Token = App.Services!.GetRequiredService<IUserSession>().Token;
        }
        public async Task<Response<Order>?> GetAllOrdersAsync(int page)
        {
            return await SendRequestAsync<Response<Order>>(HttpMethod.Get, $"order?page={page}");
        }


        public async Task<Response<Order>?> GetCustomerOrderByDate(string customerId, string startDate, string endDate)
        {
            return await SendRequestAsync<Response<Order>>(HttpMethod.Get, $"order?search={customerId}&startDate={startDate}&endDate={endDate}");
        }

        public async Task<Response<Order>?> GetOrderByDate(string startDate, string endDate, int? page = 1)
        {
            return await SendRequestAsync<Response<Order>>(HttpMethod.Get, $"order?startDate={startDate}&endDate={endDate}&page={page}");
        }

        public async Task<Response<PromotionResponse>?> GetPromotionOrder(string name)
        {
            return await SendRequestAsync<Response<PromotionResponse>>(HttpMethod.Get, $"promotions?search={name}");
        }

        public async Task<object> SendMail(string id)
        {
            return await SendRequestAsync<object>(HttpMethod.Post, $"order/{id}/send-email");
        }

        public async Task<OrderResponse> CreateOrder(OrderCreateDTO orderData)
        {
           return await SendRequestAsync<OrderResponse>(HttpMethod.Post, "order", orderData);
        }

        public async Task<List<OrderResponse>> CreateOrderDetail(OrderDetailCreateDTOList orderDetailData)
        {
           return await SendRequestAsync<List<OrderResponse>>(HttpMethod.Post, "order-detail", orderDetailData);
        
        }


    }
}
