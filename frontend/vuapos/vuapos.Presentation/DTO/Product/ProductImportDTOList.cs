using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.DTO.Product
{
    public class ProductImportDTOList
    {
        public List<ProductCreateDTO> items { get; set; }

        public ProductImportDTOList(List<ProductCreateDTO> item)
        {
            items = item;
        }
    }
}
