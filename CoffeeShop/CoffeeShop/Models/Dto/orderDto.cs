using CoffeeShop.API.Models.Domain;

namespace CoffeeShop.API.Models.Dto
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }

        public List<OrderDetailDTO> OrderDetails { get; set; }
    }
}
