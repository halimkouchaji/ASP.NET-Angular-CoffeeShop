namespace CoffeeShop.API.Models.Domain
{
    public class OrderDetail
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Guid ProductId { get; set; }
        public Products Product { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

    }
}
