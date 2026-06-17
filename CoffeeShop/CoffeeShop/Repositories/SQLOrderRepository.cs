using CoffeeShop.API.Models.Domain;
using CoffeeShop.API.DataDB;
using Microsoft.EntityFrameworkCore;


namespace CoffeeShop.API.Repositories
{
    public class SQLOrderRepository : IOrderRepository
    {
        private readonly CoffeeShopDBContext dbContext;

        public SQLOrderRepository(CoffeeShopDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await dbContext.Orders.Include(x => x.OrderDetails).ToListAsync();

        }
        public async Task<Order?> GetByIDAsync(Guid id)
        {
            return await dbContext.Orders.Include(x => x.OrderDetails).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Order> CreateOrderAsync(Order order)
        {
            order.Id = Guid.NewGuid();
            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();
            return order;
        }
        public async Task<Order?> UpdateOrderAsync(Guid id, Order order)
        {
            var existingOrder = await dbContext.Orders.Include(x => x.OrderDetails).FirstOrDefaultAsync(x => x.Id == id);

            if (existingOrder == null)
            {
                return null;
            }


            existingOrder.OrderDate = order.OrderDate;
            existingOrder.TotalPrice = order.TotalPrice;



            var detailsToDelete = existingOrder.OrderDetails.Where(existingDetail =>
!order.OrderDetails.Any(newDetail => newDetail.Id == existingDetail.Id)).ToList();

            dbContext.OrderDetails.RemoveRange(detailsToDelete);



            foreach (var detail in order.OrderDetails)
            {
                var existingDetail = existingOrder.OrderDetails
                    .FirstOrDefault(x => x.Id == detail.Id);

                if (existingDetail != null)
                {
                    // Update existing detail
                    existingDetail.ProductId = detail.ProductId;
                    existingDetail.Quantity = detail.Quantity;
                    existingDetail.UnitPrice = detail.UnitPrice;
                }
                else
                {
                    // Add new detail
                    existingOrder.OrderDetails.Add(detail);
                }
            }

            await dbContext.SaveChangesAsync();

            return existingOrder;
        }
        public async Task<Order?> DeleteOrderAsync(Guid id)
        {
            var order = await dbContext.Orders.Include(x => x.OrderDetails).FirstOrDefaultAsync(x => x.Id == id);
            if (order == null)
            {
                return null;
            }
            dbContext.Orders.Remove(order);
            await dbContext.SaveChangesAsync();
            return order;
        }
        public async Task<List<Order>> GetByDateAsync(DateTime date)
        {
            return await dbContext.Orders.Include(x => x.OrderDetails)
                .Where(x => x.OrderDate.Date == date.Date)
                .ToListAsync();
        }
        public async Task<List<Order>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            return await dbContext.Orders.Include(x => x.OrderDetails)
                .Where(x => x.OrderDate.Date >= start.Date && x.OrderDate.Date <= end.Date)
                .ToListAsync();
        }
        public async Task<List<Order>> GetLatestOrdersAsync(int count)
        {
            return await dbContext.Orders.Include(x => x.OrderDetails)
                .OrderByDescending(x => x.OrderDate)
                .Take(count)
                .ToListAsync();
        }
    }
}
