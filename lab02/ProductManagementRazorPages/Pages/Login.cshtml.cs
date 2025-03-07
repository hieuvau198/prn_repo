using BusinessObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using System.Threading.Tasks;

namespace ProductManagementRazorPages.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAccountService _accountService;

        public LoginModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var loginId = HttpContext.Session.GetInt32("Account");
            if (loginId.HasValue)
            {
                return RedirectToPage("/Products/Index");
            }
            return Page();
        }

        [BindProperty]
        public string Email { get; set; } = default!;

        [BindProperty]
        public string Password { get; set; } = default!;

        public string ErrorMessage { get; set; } = "";

        public async Task<IActionResult> OnPostAsync()
        {
            var loginId = HttpContext.Session.GetInt32("Account");
            if (loginId.HasValue)
            {
                return RedirectToPage("/Products/Index");
            }

            var memberAccount = await _accountService.AuthenticateAsync(Email, Password);

            if (memberAccount == null)
            {
                ErrorMessage = "Invalid email or password!";
                ModelState.AddModelError(string.Empty, ErrorMessage);
                return Page();
            }

            if (memberAccount.MemberRole == 1 || memberAccount.MemberRole == 2)
            {
                HttpContext.Session.SetInt32("Account", memberAccount.MemberRole);
                return RedirectToPage("/Products/Index");
            }

            ErrorMessage = "You do not have permission to access this function!";
            ModelState.AddModelError(string.Empty, ErrorMessage);
            return Page();
        }
    }
}
