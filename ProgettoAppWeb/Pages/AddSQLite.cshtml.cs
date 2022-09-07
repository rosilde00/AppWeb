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

        /**
         * Aggiunge la connection string costruita tramite il path
         * inserito alla lista delle connection string
         **/
        public async Task<IActionResult> OnPostAsync()
        {
            ConnectionStringLite.addConnectionString(lite.path);
            return RedirectToPage("./TableInfoLite");
        }
    }
}
