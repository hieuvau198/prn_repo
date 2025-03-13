using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using PRN222.Lab2.ProductStore.Repository.Models;
using PRN222.Lab2.ProductStore.Service.Services.Interface;

namespace PRN222.Lab2.ProductStore.Web.Pages.Product
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IAuthService _authService;

        public string Message { get; set; }

        public IndexModel(IProductService productService, IAuthService authService)
        {
            _productService = productService;
            _authService = authService;
        }


        public IList<PRN222.Lab2.ProductStore.Repository.Models.Product> Products { get; set; } = new List<PRN222.Lab2.ProductStore.Repository.Models.Product>();

        public async Task<IActionResult> OnGetAsync(string search = "", int pageIndex = 1, int pageSize = 10)
        {
            Message = HttpContext.Session.GetString("LoginStatus") ?? "Get Login status fail";

            // 🔒 Chặn truy cập nếu chưa đăng nhập
            if (_authService.GetCurrentUser() == null)
            {
                return RedirectToPage("/Auth/Login");
            }

            Expression<Func<PRN222.Lab2.ProductStore.Repository.Models.Product, bool>> filter = null;
            if (!string.IsNullOrEmpty(search))
            {
                filter = p => p.ProductName.Contains(search);
            }

            Products = (await _productService.GetProductsAsync(filter,
                q => q.OrderBy(p => p.ProductName), pageIndex, pageSize)).ToList();

            return Page();
        }
    }
}
