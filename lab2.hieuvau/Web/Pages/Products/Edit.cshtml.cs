using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services.BusinessModels;
using Services.Interfaces;

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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var memberRole = HttpContext.Session.GetString("MemberRole");
            if (memberRole == null)
            {
                HttpContext.Session.SetString("Message", "Please Sing-in First");
                return RedirectToPage("../Index");
            }
            else if (memberRole != "1")
            {
                HttpContext.Session.SetString("Message", "You do not have permission for this feature");
                return RedirectToPage("./Index");
            }

            Product = await _productService.GetByIdAsync(id);
            if (Product == null) return NotFound();

            var categories = await _categoryService.GetAllAsync();
            Categories = categories.Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.CategoryName }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _productService.UpdateAsync(Product);
            return RedirectToPage("Index");
        }
    }
}
