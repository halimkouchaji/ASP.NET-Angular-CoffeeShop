using CoffeeShop.API.DataDB;
using CoffeeShop.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
namespace CoffeeShop.API.Repositories
{
    public class SQLProductRepository : IProductRepository
    {
        private readonly CoffeeShopDBContext dbContext;
        
        public SQLProductRepository(CoffeeShopDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Products>> GetAllAsync()
        {
            return await dbContext.Products.ToListAsync();
        }   
        public async Task<Products?> GetByIDAsync(Guid id)
        {
            return await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Products> CreateProductAsync(Products product)
        {
            product.Id = Guid.NewGuid();
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            return product;
        }
        public async Task<Products?> UpdateProductAsync(Guid id, Products product)
        {
            var existingProduct = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (existingProduct == null)
            {
                return null;
            }
            existingProduct.Price=product.Price;
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;


            dbContext.Products.Update(existingProduct);
            await dbContext.SaveChangesAsync();
            return existingProduct;
        }
        public async Task<Products?> DeleteProductAsync(Guid id)
        {
            var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return null;
            }
            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
            return product;
        }
        public async Task<List<Products>> SearchByNameAsync(string name)
        {
            return await dbContext.Products.Where(x => x.Name.Contains(name)).ToListAsync();
        }
        public async Task<List<Products>> GetByPriceRangeAsync(double min, double max)
        {
            return await dbContext.Products.Where(x => x.Price >= min && x.Price <= max).ToListAsync();
        }
        public async Task<List<Products>> GetLowStockAsync(int threshold)
        {
            return await dbContext.Products.Where(x => x.Stock <= threshold).ToListAsync();
        }
        public async Task<Products?> UpdateStockAsync(Guid id, int quantity)
        {
            var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return null;
            }
            product.Stock += quantity;
            dbContext.Products.Update(product);
            await dbContext.SaveChangesAsync();
            return product;
        }
        public async Task<List<Products>> GetSortedByPriceAsync(bool ascending)
        {
            if (ascending)
            {
                return await dbContext.Products.OrderBy(x => x.Price).ToListAsync();
            }
            else
            {
                return await dbContext.Products.OrderByDescending(x => x.Price).ToListAsync();
            }
        }
        public async Task<List<CoffeeShop.API.Models.Dto.ProductSalesDto>> GetMostSoldProductsAsync(int count)
        {
            return await dbContext.OrderDetails
                .GroupBy(od => od.ProductId)
                .Select(g => new CoffeeShop.API.Models.Dto.ProductSalesDto
                {
                    ProductId = g.Key,
                    ProductName = g.First().Product.Name,
                    TotalSold = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(count)
                .ToListAsync();
        }
    }
}
