﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Lab2.ProductStore.Repository.Models;
using PRN222.Lab2.ProductStore.Service.Services.Interface;

namespace PRN222.Lab2.ProductStore.Web.Pages.Account
{
    public class DeleteModel : PageModel
    {
        private readonly IAccountMemberService _accountMemberService;

        public DeleteModel(IAccountMemberService accountMemberService)
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

            AccountMember = await _accountMemberService.GetByIdAsync(id.Value);
            if (AccountMember == null)
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

            var accountMember = await _accountMemberService.GetByIdAsync(id.Value);
            if (accountMember != null)
            {
                await _accountMemberService.DeleteAsync(id.Value);
            }

            return RedirectToPage("./Index");
        }
    }
}
