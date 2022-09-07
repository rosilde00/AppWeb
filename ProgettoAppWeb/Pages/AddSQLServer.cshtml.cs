using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProgettoAppWeb.Data;
using ProgettoAppWeb.Models;

namespace ProgettoAppWeb.Pages
{
    public class AddSQServerModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public SQLServerModel server { get; set; } = default!;

        public AddSQServerModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        /**
         * Aggiunge la connection string costruita tramite i parametri 
         * inseriti alla lista delle connection string
         **/
        public async Task<IActionResult> OnPostNotLocalAsync()
        {
            ConnectionStringServer.addConnectionString(server.serverAddress, server.db, server.username, server.password);
            return RedirectToPage("./TableInfoServer");
        }

        /**
         * Aggiunge la connection string con trusted connection costruita tramite i parametri 
         * inseriti alla lista delle connection string
         **/
        public async Task<IActionResult> OnPostLocalAsync()
        {
            ConnectionStringServer.addConnectionStringLocal(server.serverAddress, server.db);
            return RedirectToPage("./TableInfoServer");
        }
    }
}
