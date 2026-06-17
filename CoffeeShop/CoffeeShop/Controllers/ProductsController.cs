using Microsoft.AspNetCore.Mvc;
using CoffeeShop.API.DataDB;
using CoffeeShop.API.Models.Domain;
using CoffeeShop.API.Models.Dto;
using CoffeeShop.API.Repositories;
using Microsoft.AspNetCore.Authorization;
namespace CoffeeShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly CoffeeShopDBContext dbContext;
        private readonly IProductRepository productRepository;

        public ProductsController(CoffeeShopDBContext dbContext, IProductRepository productRepository)
        {
            this.dbContext = dbContext;
            this.productRepository = productRepository;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await productRepository.GetAllAsync();
            var productsDto = products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price

            }).ToList();
            return Ok(productsDto);
        }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await productRepository.GetByIDAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
            return Ok(productDto);
        }
        [HttpPost]

        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            var product = new Products
            {
                Id = Guid.NewGuid(),
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price
            };
            await productRepository.CreateProductAsync(product);
            var productDto1 = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, productDto1);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductDto productDto)
        {
            var product = new Products
            {
                Id = id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price
            };
            var updatedProduct = await productRepository.UpdateProductAsync(id, product);
            if (updatedProduct == null)
            {
                return NotFound();
            }
            var updatedProductDto = new ProductDto
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                Description = updatedProduct.Description,
                Price = updatedProduct.Price
            };
            return Ok(updatedProductDto);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await productRepository.DeleteProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
            return Ok(productDto);
        }
        [HttpGet]
        [Route("searchByName")]
        public async Task<IActionResult> SearchProductsByName([FromQuery] string name)
        {
            var products = await productRepository.SearchByNameAsync(name);
            var productsDto = products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            }).ToList();
            return Ok(productsDto);
        }
        [HttpGet]
        [Route("filterByPrice")]
        public async Task<IActionResult> GetProductsByPriceRange([FromQuery] double min, [FromQuery] double max)
        {
            var products = await productRepository.GetByPriceRangeAsync(min, max);
            var productsDto = products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            }).ToList();
            return Ok(productsDto);
        }
        [HttpGet]
        [Route("lowStock")]
        public async Task<IActionResult> GetLowStockProducts([FromQuery] int threshold)
        {
            var products = await productRepository.GetLowStockAsync(threshold);
            var productsDto = products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            }).ToList();
            return Ok(productsDto);
        }
        [HttpPut]
        [Route("updateStock/{id:guid}")]
        public async Task<IActionResult> UpdateProductStock( Guid id, [FromQuery] int quantity)
        {
            var updatedProduct = await productRepository.UpdateStockAsync(id, quantity);
            if (updatedProduct == null)
            {
                return NotFound();
            }
            var updatedProductDto = new ProductDto
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                Description = updatedProduct.Description,
                Price = updatedProduct.Price
            };
            return Ok(updatedProductDto);
        }
        [HttpGet]
        [Route("sortedByPrice")]
        public async Task<IActionResult> GetProductsSortedByPrice([FromQuery] bool ascending)
        {
            var products = await productRepository.GetSortedByPriceAsync(ascending);
            var productsDto = products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            }).ToList();
            return Ok(productsDto);
        }
        [HttpGet]
        [Route("mostSold")]
        public async Task<IActionResult> GetMostSoldProducts([FromQuery] int count)
        {
            var products = await productRepository.GetMostSoldProductsAsync(count);

            return Ok(products);
        }
    }
}
