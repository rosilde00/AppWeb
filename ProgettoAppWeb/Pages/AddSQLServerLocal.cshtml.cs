using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProgettoAppWeb.Data;
using ProgettoAppWeb.Models;

namespace ProgettoAppWeb.Pages
{
    public class AddSQLServerLocalModel : PageModel
    {
        private readonly ILogger<AddSQLServerLocalModel> _logger;

        [BindProperty]
        public SQLServerModel server { get; set; } = default!;

        public AddSQLServerLocalModel(ILogger<AddSQLServerLocalModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ConnectionStringServer.setConnectionStringLocal(server.serverAddress, server.db);
            _logger.LogInformation("Cambiata connection string in " + ConnectionStringServer.connectionStringLocal);
            return RedirectToPage("./TableInfoServer");
        }
    }
}

