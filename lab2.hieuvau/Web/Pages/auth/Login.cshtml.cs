using Repositories.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.BusinessModels;
using Services.Interfaces;

namespace Web.Pages.auth
{
    public class LoginModel : PageModel
    {
        private readonly IAccountService _accountService;

        public LoginModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty] public AccountModel AccountModel { get; set; }
        public string Message { get; set; }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            AccountModel account = await _accountService.AuthenticateAsync(AccountModel.EmailAddress, AccountModel.MemberPassword);

            if (account == null)
            {
                Message = "Wrong username or password";
                return Page();
            }
            else
            {
                Message = "Hi, " + account.FullName;
            }

            HttpContext.Session.SetString("MemberId", account.MemberId);

            HttpContext.Session.SetString("MemberFullName", account.FullName);

            HttpContext.Session.SetString("MemberRole", account.MemberRole.ToString());

            HttpContext.Session.SetString("Message", Message);

            return RedirectToPage("../Index");
        }


    }
}

