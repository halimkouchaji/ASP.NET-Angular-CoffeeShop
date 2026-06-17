using Microsoft.EntityFrameworkCore;
namespace CoffeeShop.API.DataDB
{
    public class CoffeeShopDBContext : DbContext
    {
        public CoffeeShopDBContext(DbContextOptions<CoffeeShopDBContext> options) : base(options)
        {
        }

        public DbSet<Models.Domain.Products> Products { get; set; }
        public DbSet<Models.Domain.Order> Orders { get; set; }
        public DbSet<Models.Domain.OrderDetail> OrderDetails { get; set; }


    }
}
