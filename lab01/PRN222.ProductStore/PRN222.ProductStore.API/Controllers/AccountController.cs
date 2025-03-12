using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRN222.ProductStore.Service.DTOs;
using PRN222.ProductStore.Service.Interfaces;

namespace PRN222.ProductStore.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountMemberService _accountMemberService;

        public AccountController(IAccountMemberService accountMemberService)
        {
            _accountMemberService = accountMemberService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountMemberService.LoginAsync(model);

                if (user != null)
                {
                    // Store user information in session
                    HttpContext.Session.SetString("UserId", user.MemberId);
                    HttpContext.Session.SetString("Username", user.FullName);
                    HttpContext.Session.SetString("UserRole", user.MemberRole.ToString());

                    return RedirectToAction("Index", "Products"); // Redirect to home page
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterAccountDto model)
        {
            if (ModelState.IsValid)
            {
                await _accountMemberService.RegisterAccountAsync(model);
                return RedirectToAction("Login");
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear session data
            return RedirectToAction("Login");
        }
    }
}
