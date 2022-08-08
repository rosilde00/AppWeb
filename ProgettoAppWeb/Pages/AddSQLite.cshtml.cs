using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProgettoAppWeb.Data;
using ProgettoAppWeb.Models;

namespace ProgettoAppWeb.Pages
{
    public class AddSQLiteModel : PageModel { 
        private readonly ILogger<AddSQLiteModel> _logger;

        [BindProperty]
        public SQLiteModel lite { get; set; } = default!;

        public AddSQLiteModel(ILogger<AddSQLiteModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ConnectionStringLite.setConnectionString(lite.path);
            _logger.LogInformation("Cambiata connection string in " + ConnectionStringLite.connectionString);
            return RedirectToPage("./TableInfoLite");
        }
    }
}
