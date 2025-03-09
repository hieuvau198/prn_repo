using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects;
using Services;

namespace ProductManagementRazorPages.Pages.Products
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
        public Product Product { get; set; } = default!;

        // Removed [BindProperty] to exclude from validation
        public string LogMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _productService.GetByIdAsync((int)id);
            if (Product == null)
            {
                return NotFound();
            }

            var categories = await _categoryService.SearchCategoriesAsync("", 1, 100);
            ViewData["CategoryId"] = new SelectList(categories.Categories, "CategoryId", "CategoryName");

            // Initial log message for page load
            LogMessage = $"Loaded product for editing at {DateTime.Now}:\n" +
                         $"ProductId: {Product.ProductId}\n" +
                         $"ProductName: {Product.ProductName ?? "null"}\n" +
                         $"CategoryId: {Product.CategoryId}\n" +
                         $"UnitsInStock: {Product.UnitsInStock?.ToString() ?? "null"}\n" +
                         $"UnitPrice: {Product.UnitPrice?.ToString() ?? "null"}\n" +
                         $"Category Object: {(Product.Category != null ? Product.Category.CategoryName : "null")}";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Log all Product details immediately after binding
            LogMessage = $"Product update request received at {DateTime.Now}:\n" +
                         $"ProductId: {Product.ProductId}\n" +
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
                LogMessage += "\nFailed to update product: Validation errors occurred.\n" + string.Join("\n", errors);
                var categories = await _categoryService.SearchCategoriesAsync("", 1, 100); // Reload categories for dropdown
                ViewData["CategoryId"] = new SelectList(categories.Categories, "CategoryId", "CategoryName");
                return Page();
            }

            try
            {
                // Fetch the Category object
                var category = await _categoryService.GetByIdAsync(Product.CategoryId);
                if (category == null)
                {
                    LogMessage += $"\nError: Category with ID {Product.CategoryId} not found.";
                    var categories = await _categoryService.SearchCategoriesAsync("", 1, 100); // Reload categories
                    ViewData["CategoryId"] = new SelectList(categories.Categories, "CategoryId", "CategoryName");
                    return Page();
                }

                // Assign the Category object
                Product.Category = category;
                LogMessage += $"\nCategory retrieved: {category.CategoryName} (ID: {category.CategoryId})";

                // Update the product
                await _productService.UpdateAsync(Product);
                LogMessage += $"\nProduct '{Product.ProductName}' was successfully updated on {DateTime.Now}.";
                return Page(); // Stay on page to show log; change to RedirectToPage("./Index") if preferred
            }
            catch (Exception ex)
            {
                LogMessage += $"\nError updating product: {ex.Message}";
                if (ex.InnerException != null)
                {
                    LogMessage += $"\nInner Exception: {ex.InnerException.Message}";
                }
                var categories = await _categoryService.SearchCategoriesAsync("", 1, 100); // Reload categories
                ViewData["CategoryId"] = new SelectList(categories.Categories, "CategoryId", "CategoryName");
                return Page();
            }
        }
    }
}