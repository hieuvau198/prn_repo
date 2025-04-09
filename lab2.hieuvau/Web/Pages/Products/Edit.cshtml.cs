using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services.BusinessModels;
using Services.Interfaces;
using System.Text.RegularExpressions;

namespace Web.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public EditModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public ProductModel Product { get; set; } = new();

        public List<SelectListItem> Categories { get; set; } = new();

        [TempData]
        public string? StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var memberRole = HttpContext.Session.GetString("MemberRole");
            if (memberRole == null)
            {
                HttpContext.Session.SetString("Message", "Please Sign-in First");
                return RedirectToPage("../Index");
            }
            else if (memberRole != "1")
            {
                HttpContext.Session.SetString("Message", "You do not have permission for this feature");
                return RedirectToPage("./Index");
            }

            Product = await _productService.GetByIdAsync(id);
            if (Product == null) return NotFound();

            await LoadCategoriesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadCategoriesAsync();

            // Additional server-side validations
            ValidateProductName();
            ValidateProductPrice();
            ValidateCategory();

            if (!ModelState.IsValid)
            {
                // Add a summary message
                ModelState.AddModelError(string.Empty, "Please correct the errors and try again.");
                return Page();
            }

            try
            {
                await _productService.UpdateAsync(Product);
                StatusMessage = "Product updated successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error updating product: {ex.Message}");
                return Page();
            }
        }

        private async Task LoadCategoriesAsync()
        {
            var categories = await _categoryService.GetAllAsync();
            Categories = categories.Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.CategoryName }).ToList();
        }

        private void ValidateProductName()
        {
            if (string.IsNullOrWhiteSpace(Product.ProductName))
                return; // Let the Required attribute handle this

            // Check if name contains any inappropriate words (example validation)
            string[] inappropriateWords = { "test", "sample", "demo", "fake" };
            if (inappropriateWords.Any(word => Product.ProductName.ToLower().Contains(word)))
            {
                ModelState.AddModelError("Product.ProductName", "Product name contains prohibited words (test, sample, demo, fake).");
            }

            // Check if product name has too many spaces
            if (Product.ProductName.Count(c => c == ' ') > 5)
            {
                ModelState.AddModelError("Product.ProductName", "Product name has too many spaces.");
            }

            // Check for consecutive spaces
            if (Regex.IsMatch(Product.ProductName, @"\s\s"))
            {
                ModelState.AddModelError("Product.ProductName", "Product name cannot contain consecutive spaces.");
            }
        }

        private void ValidateProductPrice()
        {
            if (!Product.UnitPrice.HasValue)
                return; // Let the Required attribute handle this

            // Market-specific price validation (example)
            if (Product.UnitPrice < 1.00m)
            {
                ModelState.AddModelError("Product.UnitPrice", "Products under $1.00 require special approval.");
            }

            // Check for reasonable price bounds
            if (Product.UnitPrice > 10000.00m)
            {
                ModelState.AddModelError("Product.UnitPrice", "Please verify this price. For high-value items, additional approval may be required.");
            }
        }

        private void ValidateCategory()
        {
            if (Product.CategoryId <= 0)
                return; // Let the Required/Range attribute handle this

            // Check if the category exists in our list
            if (!Categories.Any(c => c.Value == Product.CategoryId.ToString()))
            {
                ModelState.AddModelError("Product.CategoryId", "Please select a valid category from the list.");
            }
        }
    }
}