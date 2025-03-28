﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using PRN222.Lab2.ProductStore.Repository.Models;
using PRN222.Lab2.ProductStore.Service.Services.Interface;


namespace PRN222.Lab2.ProductStore.Web.Pages.Product
{
    public class DeleteModel : PageModel
    {
        private readonly IProductService _productService;
       

        public DeleteModel(IProductService productService)
        {
            _productService = productService;
          
        }

        [BindProperty]
        public PRN222.Lab2.ProductStore.Repository.Models.Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _productService.GetByIdAsync(id.Value);
            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetByIdAsync(id.Value);
            if (product != null)
            {
                await _productService.DeleteAsync(id.Value);
            }

            return RedirectToPage("./Index");
        }
    }
}
