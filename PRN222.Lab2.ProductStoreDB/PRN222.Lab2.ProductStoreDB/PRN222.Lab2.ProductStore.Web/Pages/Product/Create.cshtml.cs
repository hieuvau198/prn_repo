using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using PRN222.Lab2.ProductStore.Repository.Models;
using PRN222.Lab2.ProductStore.Service;
using PRN222.Lab2.ProductStore.Service.Services.Interface;

namespace PRN222.Lab2.ProductStore.Web.Pages.Product
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
            var categories = await _categoryService.GetCategoriesAsync();
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
            return Page();
        }

        [BindProperty]
        public PRN222.Lab2.ProductStore.Repository.Models.Product Product { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _productService.AddAsync(Product);
            return RedirectToPage("./Index");
        }
    }
}
