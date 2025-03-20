using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.BusinessModels;
using Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task OnGetAsync()
        {
            Products = await _productService.GetAllAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _productService.DeleteAsync(id);
            return RedirectToPage();
        }
    }
}
