using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Lab2.ProductStore.Service.Services.Interface;

namespace PRN222.Lab2.ProductStore.Web.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;

        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            bool loginStatus = await _authService.LoginAsync(Email, Password);
            HttpContext.Session.SetString("LoginStatus", loginStatus.ToString());
            if (loginStatus)
            {
                return RedirectToPage("/Index"); // Đăng nhập thành công
            }


            ErrorMessage = "Invalid email or password.";
            return Page();
        }
    }
}
