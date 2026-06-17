using CoffeeShop.API.Models.Domain; 

namespace CoffeeShop.API.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order?> GetByIDAsync(Guid id);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order?> UpdateOrderAsync(Guid id, Order order);
        Task<Order?> DeleteOrderAsync(Guid id);
        Task<List<Order>> GetByDateAsync(DateTime date);
        Task<List<Order>> GetByDateRangeAsync(DateTime start, DateTime end);
        Task<List<Order>> GetLatestOrdersAsync(int count);
    }
}
