using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Lab2.ProductStore.Repository.Models;
using PRN222.Lab2.ProductStore.Service.Services.Interface;

namespace PRN222.Lab2.ProductStore.Web.Pages.Account
{
    public class CreateModel : PageModel
    {
        private readonly IAccountMemberService _accountMemberService;

        public CreateModel(IAccountMemberService accountMemberService)
        {
            _accountMemberService = accountMemberService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public AccountMember AccountMember { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _accountMemberService.AddAsync(AccountMember);

            return RedirectToPage("./Index");
        }
    }
}
