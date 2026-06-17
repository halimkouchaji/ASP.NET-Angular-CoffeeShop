using CoffeeShop.API.Models.Domain;
using CoffeeShop.API.DataDB;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.API.Repositories
{
    public class SQLOrderDetailRepository : IOrderDetailRepository
    {
        public readonly CoffeeShopDBContext dBContext;
        public SQLOrderDetailRepository(CoffeeShopDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<List<OrderDetail>> GetAllAsync()
        {
            return await dBContext.OrderDetails.Include(od => od.Order).Include(od => od.Product).ToListAsync();
        }
        public async Task<OrderDetail?> GetByIDAsync(Guid id)
        {
            return await dBContext.OrderDetails.Include(od => od.Order).Include(od => od.Product).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<OrderDetail> CreateOrderDetailAsync(OrderDetail orderDetail)
        {
            orderDetail.Id = Guid.NewGuid();
            await dBContext.OrderDetails.AddAsync(orderDetail);
            await dBContext.SaveChangesAsync();
            return orderDetail;
        }
        public async Task<OrderDetail?> UpdateOrderDetailAsync(Guid id, OrderDetail orderDetail)
        {
            var existingOrderDetail = await dBContext.OrderDetails
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingOrderDetail == null)
            {
                return null;
            }

            if (!await dBContext.Orders.AnyAsync(o => o.Id == orderDetail.OrderId))
            {
                throw new Exception("Order does not exist");
            }

            if (!await dBContext.Products.AnyAsync(p => p.Id == orderDetail.ProductId))
            {
                throw new Exception("Product does not exist");
            }

            existingOrderDetail.OrderId = orderDetail.OrderId;
            existingOrderDetail.ProductId = orderDetail.ProductId;
            existingOrderDetail.Quantity = orderDetail.Quantity;
            existingOrderDetail.UnitPrice = orderDetail.UnitPrice;

            await dBContext.SaveChangesAsync();

            return existingOrderDetail;
        }
        public async Task<OrderDetail?> DeleteOrderDetailAsync(Guid id)
        {
            var orderDetail = await dBContext.OrderDetails.FirstOrDefaultAsync(x => x.Id == id);
            if (orderDetail == null)
            {
                return null;
            }
            dBContext.OrderDetails.Remove(orderDetail);
            await dBContext.SaveChangesAsync();
            return orderDetail;
        }
        public async Task<List<OrderDetail>> GetByOrderIdAsync(Guid orderId)
        {
            return await dBContext.OrderDetails
                .Where(od => od.OrderId == orderId)
                .Include(od => od.Product)
                .ToListAsync();
        }
        public async Task<List<OrderDetail>> GetByProductIdAsync(Guid productId)
        {
            return await dBContext.OrderDetails
                .Where(od => od.ProductId == productId)
                .Include(od => od.Order)
                .ToListAsync();
        }
        public async Task<OrderDetail?> UpdateQuantityAsync(Guid id, int quantity)
        {
            var existingOrderDetail = await dBContext.OrderDetails
                .FirstOrDefaultAsync(x => x.Id == id);
            if (existingOrderDetail == null)
            {
                return null;
            }
            existingOrderDetail.Quantity = quantity;
            await dBContext.SaveChangesAsync();
            return existingOrderDetail;
        }
        public async Task<double> CalculateTotalAsync(Guid orderId)
        {
            var orderDetails = await dBContext.OrderDetails
                .Where(od => od.OrderId == orderId)
                .ToListAsync();
            double total = 0;
            foreach (var orderDetail in orderDetails)
            {
                total += orderDetail.Quantity * orderDetail.UnitPrice;
            }
            return total;
        }
    }
}