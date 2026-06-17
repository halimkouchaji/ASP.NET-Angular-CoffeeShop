
namespace CoffeeShop.API.Models.Domain
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
       
        
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
