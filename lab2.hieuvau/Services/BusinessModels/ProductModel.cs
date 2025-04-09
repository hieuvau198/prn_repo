using System.ComponentModel.DataAnnotations;

namespace Services.BusinessModels
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [MinLength(10, ErrorMessage = "Product name must be at least 10 characters long")]
        [MaxLength(100, ErrorMessage = "Product name cannot exceed 100 characters")]
        [RegularExpression(@"^([A-Z0-9][a-zA-Z0-9]*(\s)?)+$",
            ErrorMessage = "Product name must start with a capital letter or number for each word. No special characters allowed.")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a category")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Units in stock is required")]
        [Range(0, 10000, ErrorMessage = "Units in stock must be between 0 and 10,000")]
        [Display(Name = "Units In Stock")]
        public short? UnitsInStock { get; set; }

        [Required(ErrorMessage = "Unit price is required")]
        [Range(0.01, 100000, ErrorMessage = "Unit price must be between $0.01 and $100,000")]
        [DataType(DataType.Currency)]
        [Display(Name = "Unit Price")]
        public decimal? UnitPrice { get; set; }

        public string? CategoryName { get; set; }
    }
}