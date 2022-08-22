using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProgettoAppWeb.Data;
using ProgettoAppWeb.Models;

namespace ProgettoAppWeb.Pages
{
    public class AddSQLServerModel : PageModel
    {
        private readonly ILogger<AddSQLServerModel> _logger;

        [BindProperty]
        public SQLServerModel server { get; set; } = default!;

        public AddSQLServerModel(ILogger<AddSQLServerModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ConnectionStringServer.setConnectionString(server.serverAddress, server.db, server.username, server.password);
            _logger.LogInformation("Cambiata connection string in " + ConnectionStringServer.connectionString);
            return RedirectToPage("./TableInfoServer");
        }
    }
}
