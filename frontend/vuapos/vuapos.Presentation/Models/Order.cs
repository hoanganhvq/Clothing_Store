using Microsoft.Windows.PushNotifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Windows.Web.AtomPub;

namespace vuapos.Presentation.Models
{
    public class Order
    {
        [JsonPropertyName("orderId")]
        public int Order_Id { get; set; }

        [JsonPropertyName("orderDate")]
        public string Order_Date { get; set; }
        public int Customer_Id { get; set; }
        [JsonPropertyName("totalAmount")]
        public decimal Total_Amount { get; set; }
        public string Staff_Id { get; set; }
        public Customer customer { get; set; } = new Customer();

        public Staff staff { get; set; } = new Staff();

        public Promotion promotion { get; set; } = new Promotion();
        public string Order_status { get; set; } = "Đã thanh toán";

        [JsonPropertyName("isCash")]
        public bool Is_Cash { get; set; }

        [JsonPropertyName("isUseCustomerPoint")]
        public bool Is_Use_Customer_Point { get; set; }

        [JsonPropertyName("pointDiscount")]
        public decimal? Point_Discount { get; set; }


        [JsonPropertyName("items")]
        public ObservableCollection<OrderDetail> OrderDetails { get; set; } = new ObservableCollection<OrderDetail>();
        
        
        public string OrderDate_Time_Format()
        {
            if (DateTime.TryParse(Order_Date, out DateTime orderDate))
            {
                return orderDate.ToString("dd/MM/yyyy, hh:mm tt", new System.Globalization.CultureInfo("vi-VN"));
            }
            return Order_Date;
        }

        public string OrderDateFormatted =>
     DateTime.TryParse(Order_Date, out DateTime orderDate)
     ? orderDate.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("vi-VN"))
     : Order_Date;




    }
}
