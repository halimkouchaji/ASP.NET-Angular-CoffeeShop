using CoffeeShop.API.DataDB;
using CoffeeShop.API.Models.Dto;
using CoffeeShop.API.Repositories;
using CoffeeShop.API.Models.Domain;
using Microsoft.AspNetCore.Mvc;
namespace CoffeeShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;
        public readonly CoffeeShopDBContext dbContext;

        public OrderController(IOrderRepository orderRepository, CoffeeShopDBContext dbContext)
        {
            this.orderRepository = orderRepository;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await orderRepository.GetAllAsync();

            var ordersDto = orders.Select(order => new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                OrderDetails = order.OrderDetails.Select(detail => new OrderDetailDTO
                {
                    Id = detail.Id,
                    ProductId = detail.ProductId,
                    OrderId = detail.OrderId,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                }).ToList()
            }).ToList();

            return Ok(ordersDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await orderRepository.GetByIDAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            var orderDto = new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                OrderDetails = order.OrderDetails.Select(detail => new OrderDetailDTO
                {
                    Id = detail.Id,
                    ProductId = detail.ProductId,
                    OrderId = detail.OrderId,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                }).ToList()
            };

            return Ok(orderDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDto)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderDate = orderDto.OrderDate,
                TotalPrice = orderDto.TotalPrice,
                OrderDetails = orderDto.OrderDetails.Select(detail => new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                }).ToList()
            };
            var createdOrder = await orderRepository.CreateOrderAsync(order);
            var createdOrderDto = new OrderDTO
            {
                Id = createdOrder.Id,
                OrderDate = createdOrder.OrderDate,
                TotalPrice = createdOrder.TotalPrice,
                OrderDetails = createdOrder.OrderDetails.Select(detail => new OrderDetailDTO
                {
                    Id = detail.Id,
                    OrderId = detail.OrderId,
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                }).ToList()
            };
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrderDto);
        }
        [HttpPut]
        [Route("update/order/{id:guid}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] OrderDTO orderDto)
        {
            var order = new Order
            {
                Id = id,
                OrderDate = orderDto.OrderDate,
                TotalPrice = orderDto.TotalPrice,
                OrderDetails = orderDto.OrderDetails.Select(detail => new OrderDetail
                {
                    Id = detail.Id,
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                }).ToList()
            };
            var updatedOrder = await orderRepository.UpdateOrderAsync(id, order);
            if (updatedOrder == null)
            {
                return NotFound();
            }
            var updatedOrderDto = new OrderDTO
            {
                Id = updatedOrder.Id,
                OrderDate = updatedOrder.OrderDate,
                TotalPrice = updatedOrder.TotalPrice,
                OrderDetails = updatedOrder.OrderDetails.Select(detail => new OrderDetailDTO
                {
                    Id = detail.Id,
                    OrderId = detail.OrderId,
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                }).ToList()
            };
            return Ok(updatedOrderDto);
        }

        [HttpDelete]
        [Route("delete/order/{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var deleted = await orderRepository.DeleteOrderAsync(id);
            if (deleted == null)
            {
                return NotFound();
            }
            return Ok();
        }
        [HttpGet("date")]
        public async Task<IActionResult> GetOrdersByDate([FromQuery] DateTime date)
        {
            var orders = await orderRepository.GetByDateAsync(date);
            var ordersDto = orders.Select(order => new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                OrderDetails = order.OrderDetails.Select(detail => new OrderDetailDTO
                {
                    Id = detail.Id,
                    ProductId = detail.ProductId,
                    OrderId = detail.OrderId,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                }).ToList()
            }).ToList();
            return Ok(ordersDto);
        }
        [HttpGet]
        [Route("date-range")]
        public async Task<IActionResult> GetOrdersByDateRange([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var orders = await orderRepository.GetByDateRangeAsync(start, end);
            var ordersDto = orders.Select(order => new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                OrderDetails = order.OrderDetails.Select(detail => new OrderDetailDTO
                {
                    Id = detail.Id,
                    ProductId = detail.ProductId,
                    OrderId = detail.OrderId,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                }).ToList()
            }).ToList();
            return Ok(ordersDto);
        }
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestOrders([FromQuery] int count=5)
        {
            var orders = await orderRepository.GetLatestOrdersAsync(count);
            var ordersDto = orders.Select(order => new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                OrderDetails = order.OrderDetails.Select(detail => new OrderDetailDTO
                {
                    Id = detail.Id,
                    ProductId = detail.ProductId,
                    OrderId = detail.OrderId,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                }).ToList()
            }).ToList();
            return Ok(ordersDto);
        }
    }
}