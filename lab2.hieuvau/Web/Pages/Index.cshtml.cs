using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public string Message { get; set; }
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            string memberId = HttpContext.Session.GetString("MemberId");
            if (string.IsNullOrEmpty(memberId))
            {
                return RedirectToPage("/Login/Index");
            }

            Message = HttpContext.Session.GetString("Message") ?? "Get Message failed";

            return Page();
        }
    }
}
