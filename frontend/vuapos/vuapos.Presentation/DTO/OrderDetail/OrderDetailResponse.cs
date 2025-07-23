using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace vuapos.Presentation.DTO.Order
{
    public class OrderDetailResponse
    {


        [JsonPropertyName("orderId")]
        public int Order_id { get; set; }


        [JsonPropertyName("productId")]
        public int Product_id { get; set; }



        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}
