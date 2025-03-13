using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN222.Lab2.ProductStore.Repository.Models;
using PRN222.Lab2.ProductStore.Service;
using PRN222.Lab2.ProductStore.Service.Services.Interface;

namespace PRN222.Lab2.ProductStore.Web.Pages.Account
{
    public class EditModel : PageModel
    {
        private readonly IAccountMemberService _accountMemberService;

        public EditModel(IAccountMemberService accountMemberService)
        {
            _accountMemberService = accountMemberService;
        }

        [BindProperty]
        public AccountMember AccountMember { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountMember = await _accountMemberService.GetByIdAsync(id.Value);
            if (accountMember == null)
            {
                return NotFound();
            }

            AccountMember = accountMember;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                Console.WriteLine(AccountMember.FullName);
                await _accountMemberService.UpdateAsync(AccountMember);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AccountMemberExists(AccountMember.MemberId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> AccountMemberExists(int id)
        {
            return await _accountMemberService.GetByIdAsync(id) != null;
        }
    }
}
