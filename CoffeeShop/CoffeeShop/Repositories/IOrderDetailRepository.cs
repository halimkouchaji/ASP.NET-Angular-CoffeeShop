using CoffeeShop.API.Models.Domain;

namespace CoffeeShop.API.Repositories
{
    public interface IOrderDetailRepository
    {
        Task<List<OrderDetail>> GetAllAsync();
        Task<OrderDetail?> GetByIDAsync(Guid id);
        Task<OrderDetail> CreateOrderDetailAsync(OrderDetail orderDetail);
        Task<OrderDetail?> UpdateOrderDetailAsync(Guid id, OrderDetail orderDetail);
        Task<OrderDetail?> DeleteOrderDetailAsync(Guid id);
        Task<List<OrderDetail>> GetByOrderIdAsync(Guid orderId);
        Task<List<OrderDetail>> GetByProductIdAsync(Guid productId);
        Task<OrderDetail?> UpdateQuantityAsync(Guid id, int quantity);
        Task<double> CalculateTotalAsync(Guid orderId);
    }
}
