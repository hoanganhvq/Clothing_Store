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
        [JsonPropertyName("id")]
        public int Order_Id { get; set; }
        public string Order_Date { get; set; }
        public int Customer_Id { get; set; }
        public decimal Total_Amount { get; set; }
        public string Staff_Id { get; set; }
        public Customer customer { get; set; } = new Customer();

        public Staff staff { get; set; } = new Staff();
        public string Order_status { get; set; } = "Đã thanh toán";
        public ObservableCollection<OrderDetail> OrderDetails { get; set; } = new ObservableCollection<OrderDetail>();

    }
}
