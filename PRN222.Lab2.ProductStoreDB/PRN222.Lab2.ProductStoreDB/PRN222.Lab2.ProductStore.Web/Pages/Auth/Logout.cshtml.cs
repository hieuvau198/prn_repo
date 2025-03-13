using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Lab2.ProductStore.Service.Services.Interface;

namespace PRN222.Lab2.ProductStore.Web.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        private readonly IAuthService _authService;

        public LogoutModel(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult OnGet()
        {
            // Nếu bạn muốn hiển thị trang xác nhận logout, giữ phần này
            return Page();
        }

        public IActionResult OnPost()
        {
            // Gọi phương thức logout từ AuthService nếu có
            _authService.Logout();

            // Xóa session
            HttpContext.Session.Remove("LoginStatus");
            HttpContext.Session.Clear(); // Xóa toàn bộ session để đảm bảo

            // Xóa cookie
            Response.Cookies.Delete("LoginStatus");

            // Thêm thông báo đăng xuất thành công
            TempData["Message"] = "You have been logged out successfully.";

            // Điều hướng về trang Index
            return RedirectToPage("/Index");
        }
    }
}