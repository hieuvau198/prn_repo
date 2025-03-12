namespace PRN222.ProductStore.Service.DTOs
{
    public class CreateProductDto
    {
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public short? UnitsInStock { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
