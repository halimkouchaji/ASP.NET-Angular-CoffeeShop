using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.API.DataDB
{
    public class CoffeeShopAuthDBContext : IdentityDbContext
    {
        public CoffeeShopAuthDBContext(DbContextOptions<CoffeeShopAuthDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var roles = new List<IdentityRole>()
    {
        new IdentityRole()
        {
            Id = Guid.Parse("3f2504e0-4f89-41d3-9a0c-0305e82c3301").ToString(),
            ConcurrencyStamp = Guid.Parse("3f2504e0-4f89-41d3-9a0c-0305e82c3301").ToString(),
            Name = "Admin",
            NormalizedName = "ADMIN"
        },
        new IdentityRole()
        {
            Id = Guid.Parse("a1b2c3d4-9f8e-4a7b-b6c5-1234567890ab").ToString(),
            ConcurrencyStamp = Guid.Parse("a1b2c3d4-9f8e-4a7b-b6c5-1234567890ab").ToString(),
            Name = "User",
            NormalizedName = "USER"
        }
    };
            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
