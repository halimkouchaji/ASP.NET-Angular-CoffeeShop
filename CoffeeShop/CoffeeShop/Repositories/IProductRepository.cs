using CoffeeShop.API.Models.Domain;

namespace CoffeeShop.API.Repositories
{
    public interface IProductRepository
    {
        Task<List<Products>> GetAllAsync();
        Task<Products?> GetByIDAsync(Guid id);
        Task<Products> CreateProductAsync(Products product);
        Task<Products?> UpdateProductAsync(Guid id,Products product);
        Task<Products?> DeleteProductAsync(Guid id);
        Task<List<Products>> SearchByNameAsync(string name);
        Task<List<Products>> GetByPriceRangeAsync(double min, double max);
        Task<List<Products>> GetLowStockAsync(int threshold);
        Task<Products?> UpdateStockAsync(Guid id, int quantity);
        Task<List<Products>> GetSortedByPriceAsync(bool ascending);
        Task<List<CoffeeShop.API.Models.Dto.ProductSalesDto>> GetMostSoldProductsAsync(int count);
    }
}
