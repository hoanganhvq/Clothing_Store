using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.Models;

namespace vuapos.Presentation.DAO.Interface
{
    public interface IProductDao
    {
        Task<Product> GetProductByIdAsync(string productId);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(string productId);
        Task<IEnumerable<Product>> SearchProductsByNameAsync(string name);
    }
}
