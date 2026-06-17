namespace CoffeeShop.API.Models.Dto
{
    public class ProductSalesDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalSold { get; set; }
    }
}
