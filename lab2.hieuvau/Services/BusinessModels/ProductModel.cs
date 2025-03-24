using System.ComponentModel.DataAnnotations;

namespace Services.BusinessModels
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        [Required]
        [MinLength(10)]
        [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9]*(\s)?)+$", ErrorMessage = "Each word must start with a capital letter or number, and no special characters allowed.")]
        public string ProductName { get; set; } = string.Empty;
        [Required]
        public int CategoryId { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Units in stock must be 0 or greater.")]
        public short? UnitsInStock { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0.")]
        public decimal? UnitPrice { get; set; }
        public string? CategoryName { get; set; }
    }
}
