using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Lab2.ProductStore.Repository.Models;
using PRN222.Lab2.ProductStore.Service.Services.Interface;

namespace PRN222.Lab2.ProductStore.Web.Pages.Account
{
    public class IndexModel : PageModel
    {
        private readonly IAccountMemberService _accountMemberService;
        private readonly IAuthService _authService; // Thêm AuthService để kiểm tra Session

        public IndexModel(IAccountMemberService accountMemberService, IAuthService authService)
        {
            _accountMemberService = accountMemberService;
            _authService = authService;
        }

        public IList<AccountMember> AccountMembers { get; set; } = new List<AccountMember>();

        public async Task<IActionResult> OnGetAsync(string search = "", int pageIndex = 1, int pageSize = 10)
        {
            // 🛑 Kiểm tra Session trước khi vào trang
            var user = _authService.GetCurrentUser();
            if (user == null)
            {
                return RedirectToPage("/Auth/Login"); // Chưa đăng nhập → Chuyển hướng về Login
            }

            AccountMembers = (await _accountMemberService.GetAccountMembersAsync(
                a => string.IsNullOrEmpty(search) || a.FullName.Contains(search),
                q => q.OrderBy(a => a.FullName),
                pageIndex,
                pageSize)).ToList();

            return Page();
        }
    }
}
