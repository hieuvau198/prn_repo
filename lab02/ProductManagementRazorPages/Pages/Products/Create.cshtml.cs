using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using Repositories.Entities;

namespace ProductManagementRazorPages.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public CreateModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var categories = await _categoryService.SearchCategoriesAsync("", 1, 100); // Fetch categories
            ViewData["CategoryId"] = new SelectList(categories.Categories, "CategoryId", "CategoryName");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        // Removed [BindProperty] to exclude from validation
        public string LogMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            // Log all Product details immediately after binding
            LogMessage = $"Product creation request received at {DateTime.Now}:\n" +
                         $"ProductName: {Product.ProductName ?? "null"}\n" +
                         $"CategoryId: {Product.CategoryId}\n" +
                         $"UnitsInStock: {Product.UnitsInStock?.ToString() ?? "null"}\n" +
                         $"UnitPrice: {Product.UnitPrice?.ToString() ?? "null"}\n" +
                         $"Category Object: {(Product.Category != null ? Product.Category.CategoryName : "null")}";

            // Clear the Category validation error since we handle it manually
            if (ModelState.ContainsKey("Product.Category") && Product.CategoryId > 0)
            {
                ModelState.Remove("Product.Category"); // Remove the Category-specific error
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(m => m.Value.Errors.Count > 0)
                    .Select(m => $"{m.Key}: {string.Join(", ", m.Value.Errors.Select(e => e.ErrorMessage))}");
                LogMessage += "\nFailed to create product: Validation errors occurred.\n" + string.Join("\n", errors);
                return Page();
            }

            try
            {
                // Fetch the Category object
                var category = await _categoryService.GetByIdAsync(Product.CategoryId);
                if (category == null)
                {
                    LogMessage += $"\nError: Category with ID {Product.CategoryId} not found.";
                    return Page();
                }

                // Assign the Category object
                Product.Category = category;
                LogMessage += $"\nCategory retrieved: {category.CategoryName} (ID: {category.CategoryId})";

                // Add the product
                await _productService.AddAsync(Product);
                LogMessage += $"\nProduct '{Product.ProductName}' was successfully created on {DateTime.Now}.";
            }
            catch (Exception ex)
            {
                LogMessage += $"\nError creating product: {ex.Message}";
                if (ex.InnerException != null)
                {
                    LogMessage += $"\nInner Exception: {ex.InnerException.Message}";
                }
            }

            return Page();
        }
    }
}