using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace vuapos.Presentation.DTO.Product
{
    public class ProductCreateDTO
    {
        public required String product_code { get; set; }
        public required String product_name { get; set; }
        public required String category_id { get; set; }
        public required Decimal price { get; set; }
        public required Decimal cost_price { get; set; }
        public required Int32 stock_quantity { get; set; }

        public required int discount { get; set; }
        public required string image_path { get; set; } = string.Empty;

        //public required string image_path { get; set; }
    }
}
