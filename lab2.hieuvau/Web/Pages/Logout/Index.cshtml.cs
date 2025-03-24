using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Logout
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Clear(); // Clear all session values
            return RedirectToPage("/Login/Index"); // Redirect to login page
        }
    }
}
