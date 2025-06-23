using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.Models;

namespace vuapos.Presentation.DAO.Interface
{
    public interface IOrderDetailDao
    {
        Task<OrderDetail> GetOrderDetailByIdAsync(string orderDetailId);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(string orderId);
        Task AddOrderDetailAsync(OrderDetail orderDetail);
        Task UpdateOrderDetailAsync(OrderDetail orderDetail);
        Task DeleteOrderDetailAsync(string orderDetailId);
        // Phương thức đặc biệt để thêm sản phẩm mới vào đơn hàng
        Task<OrderDetail> AddProductToOrderAsync(string orderId, string productId, int quantity);
        // Phương thức đặc biệt để cập nhật số lượng (và tự động xóa nếu quantity = 0)
        Task UpdateOrderDetailQuantityAsync(string orderDetailId, int newQuantity);
    }
}
