using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.BusinessModels;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public IndexModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IEnumerable<ProductModel> Products { get; set; } = new List<ProductModel>();

        // Filter
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? StockBelow { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? PriceAbove { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? FilterCategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();

        // Paging
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        public int TotalPages { get; set; }

        // Session
        public string Message { get; set; }
        public string MemberRole { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string memberId = HttpContext.Session.GetString("MemberId");
            if (string.IsNullOrEmpty(memberId))
            {
                return RedirectToPage("/Login/Index");
            }

            var categoryList = await _categoryService.GetAllAsync();
            Categories = categoryList
                .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.CategoryName })
                .ToList();

            Message = HttpContext.Session.GetString("Message");
            HttpContext.Session.Remove("Message");
            MemberRole = HttpContext.Session.GetString("MemberRole");

            var allProducts = await _productService.GetAllAsync();

            // Auto-search in both ProductName and CategoryName
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                allProducts = allProducts.Where(p =>
                    (p.ProductName != null && p.ProductName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (p.CategoryName != null && p.CategoryName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                );
            }

            // Stock filter
            if (StockBelow.HasValue)
            {
                allProducts = allProducts.Where(p => p.UnitsInStock < StockBelow.Value);
            }

            // Price filter
            if (PriceAbove.HasValue)
            {
                allProducts = allProducts.Where(p => p.UnitPrice.HasValue && p.UnitPrice > PriceAbove.Value);
            }

            // Category filter
            if (FilterCategoryId.HasValue)
            {
                allProducts = allProducts.Where(p => p.CategoryId == FilterCategoryId.Value);
            }

            // Order by ID descending
            var sorted = allProducts.OrderByDescending(p => p.ProductId).ToList();

            // Pagination
            TotalPages = (int)Math.Ceiling(sorted.Count / (double)PageSize);
            Products = sorted.Skip((PageNumber - 1) * PageSize).Take(PageSize);

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var memberRole = HttpContext.Session.GetString("MemberRole");
            if (memberRole == null)
            {
                HttpContext.Session.SetString("Message", "Please Sign-in First");
                return RedirectToPage();
            }
            else if (memberRole != "1")
            {
                HttpContext.Session.SetString("Message", "You do not have permission for this feature");
                return RedirectToPage();
            }

            await _productService.DeleteAsync(id);
            return RedirectToPage(new { pageNumber = PageNumber, pageSize = PageSize });
        }
    }
}