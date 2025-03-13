using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Repositories.Entities;

namespace ProductManagementRazorPages.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public IList<Product> Product { get; private set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Account")))
            {
                var result = await _productService.SearchProductsAsync("", 1, 20); // Get first 20 products
                Product = result.Products.ToList();
                return Page();
            }
            return RedirectToPage("/Login");
        }
    }
}
