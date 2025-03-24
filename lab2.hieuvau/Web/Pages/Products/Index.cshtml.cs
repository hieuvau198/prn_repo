using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.BusinessModels;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Web.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public IEnumerable<ProductModel> Products { get; set; } = new List<ProductModel>();

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;  // Renamed from Page to PageNumber

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        public int TotalPages { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Get current page items
            Products = await _productService.GetPagedAsync(PageNumber, PageSize);  // Use PageNumber instead of Page

            // Calculate total pages by getting total count of items
            var allProducts = await _productService.GetAllAsync();
            var totalItems = allProducts.Count();
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _productService.DeleteAsync(id);
            return RedirectToPage(new { pageNumber = PageNumber, pageSize = PageSize });  // Use pageNumber instead of page
        }
    }
}