namespace CoffeeShop.API.Models.Domain
{
    public class Products
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; } 
        public string? Description { get; set; } 
        public int Stock { get; set; } 
    }
}
