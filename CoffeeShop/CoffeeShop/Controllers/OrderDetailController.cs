using CoffeeShop.API.Models.Domain;
using CoffeeShop.API.Models.Dto;
using CoffeeShop.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailRepository orderDetailRepository;
        private readonly DataDB.CoffeeShopDBContext dBContext;

        public OrderDetailController(IOrderDetailRepository orderDetailRepository, DataDB.CoffeeShopDBContext dBContext)
        {
            this.orderDetailRepository = orderDetailRepository;
            this.dBContext = dBContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrderDetails()
        {
            var orderDetails = await orderDetailRepository.GetAllAsync();
            var orderDetailsDTO = orderDetails.Select(od => new OrderDetailDTO
            {
                Id = od.Id,
                OrderId = od.OrderId,
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice
            }).ToList();

            return Ok(orderDetailsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetOrderDetailById(Guid id)
        {
            var orderDetail = await orderDetailRepository.GetByIDAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            var orderDetailDTO = new OrderDetailDTO
            {
                Id = orderDetail.Id,
                OrderId = orderDetail.OrderId,
                ProductId = orderDetail.ProductId,
                Quantity = orderDetail.Quantity,
                UnitPrice = orderDetail.UnitPrice
            };
            return Ok(orderDetailDTO);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrderDetail([FromBody] OrderDetailDTO orderDetailDTO)
        {
            var orderDetail = new OrderDetail
            {
                OrderId = orderDetailDTO.OrderId,
                ProductId = orderDetailDTO.ProductId,
                Quantity = orderDetailDTO.Quantity,
                UnitPrice = orderDetailDTO.UnitPrice
            };
            var createdOrderDetail = await orderDetailRepository.CreateOrderDetailAsync(orderDetail);
            var createdOrderDetailDTO = new OrderDetailDTO
            {
                Id = createdOrderDetail.Id,
                OrderId = createdOrderDetail.OrderId,
                ProductId = createdOrderDetail.ProductId,
                Quantity = createdOrderDetail.Quantity,
                UnitPrice = createdOrderDetail.UnitPrice
            };
            return CreatedAtAction(nameof(GetOrderDetailById), new { id = createdOrderDetailDTO.Id }, createdOrderDetailDTO);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateOrderDetail(Guid id, [FromBody] OrderDetailDTO orderDetailDTO)
        {
            var orderDetail = new OrderDetail
            {
                OrderId = orderDetailDTO.OrderId,
                ProductId = orderDetailDTO.ProductId,
                Quantity = orderDetailDTO.Quantity,
                UnitPrice = orderDetailDTO.UnitPrice
            };

            try
            {
                var updatedOrderDetail = await orderDetailRepository.UpdateOrderDetailAsync(id, orderDetail);

                if (updatedOrderDetail == null)
                {
                    return NotFound();
                }

                var updatedOrderDetailDTO = new OrderDetailDTO
                {
                    Id = updatedOrderDetail.Id,
                    OrderId = updatedOrderDetail.OrderId,
                    ProductId = updatedOrderDetail.ProductId,
                    Quantity = updatedOrderDetail.Quantity,
                    UnitPrice = updatedOrderDetail.UnitPrice
                };

                return Ok(updatedOrderDetailDTO);
            }
            catch (DbUpdateException)
            {
                return BadRequest("Invalid OrderId or ProductId.");
            }
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteOrderDetail(Guid id)
        {
            var deletedOrderDetail = await orderDetailRepository.DeleteOrderDetailAsync(id);
            if (deletedOrderDetail == null)
            {
                return NotFound();
            }
            var deletedOrderDetailDTO = new OrderDetailDTO
            {
                Id = deletedOrderDetail.Id,
                OrderId = deletedOrderDetail.OrderId,
                ProductId = deletedOrderDetail.ProductId,
                Quantity = deletedOrderDetail.Quantity,
                UnitPrice = deletedOrderDetail.UnitPrice
            };
            return Ok(deletedOrderDetailDTO);
        }
        
        [HttpGet("order/{orderId:guid}")]
        public async Task<IActionResult> GetOrderDetailsByOrderId(Guid orderId)
        {
            var orderDetails = await orderDetailRepository.GetByOrderIdAsync(orderId);
            var orderDetailsDTO = orderDetails.Select(od => new OrderDetailDTO
            {
                Id = od.Id,
                OrderId = od.OrderId,
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice
            }).ToList();
            return Ok(orderDetailsDTO);
        }
        [HttpGet]
        [Route("product/{productId:guid}")]
        public async Task<IActionResult> GetOrderDetailsByProductId(Guid productId)
        {
            var orderDetails = await orderDetailRepository.GetByProductIdAsync(productId);
            var orderDetailsDTO = orderDetails.Select(od => new OrderDetailDTO
            {
                Id = od.Id,
                OrderId = od.OrderId,
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice
            }).ToList();
            return Ok(orderDetailsDTO);

        }
        [HttpPut]
        [Route("quantity/{id:guid}")]
        public async Task<IActionResult> UpdateOrderDetailQuantity(Guid id, [FromQuery] int quantity)
        {
            var updatedOrderDetail = await orderDetailRepository.UpdateQuantityAsync(id, quantity);
            if (updatedOrderDetail == null)
            {
                return NotFound();
            }
            var updatedOrderDetailDTO = new OrderDetailDTO
            {
                Id = updatedOrderDetail.Id,
                OrderId = updatedOrderDetail.OrderId,
                ProductId = updatedOrderDetail.ProductId,
                Quantity = updatedOrderDetail.Quantity,
                UnitPrice = updatedOrderDetail.UnitPrice
            };
            return Ok(updatedOrderDetailDTO);
        }
        [HttpGet("order/{orderId:guid}/total")]
        public async Task<IActionResult> CalculateTotal(Guid orderId)
        {
            var total = await orderDetailRepository.CalculateTotalAsync(orderId);
            return Ok(total);
        }
    }
}
