using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.BusinessModels;
using Services.Interfaces;

namespace Web.Pages.Login
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;

        public IndexModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty] public AccountModel AccountModel { get; set; }
        public string Message { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            string memberId = HttpContext.Session.GetString("MemberId");
            if (!string.IsNullOrEmpty(memberId))
            {
                return RedirectToPage("../Products/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            AccountModel account = await _accountService.AuthenticateAsync(AccountModel.EmailAddress, AccountModel.MemberPassword);

            if (account == null)
            {
                Message = "Wrong username or password";
                return Page();
            }
            else if (account.MemberRole == 1)
            {
                Message = "Hi, " +  "Manager " + account.FullName;
            }
            else if (account.MemberRole == 2)
            {
                Message = "Hi, " + "Staff " + account.FullName;
            }
            else
            {
                Message = "You don't have permission to access";
                return Page();
            }

            HttpContext.Session.SetString("MemberId", account.MemberId);

            HttpContext.Session.SetString("MemberFullName", account.FullName);

            HttpContext.Session.SetString("MemberRole", account.MemberRole.ToString());

            HttpContext.Session.SetString("Message", Message);

            return RedirectToPage("../Products/Index");
        }


    }
}
