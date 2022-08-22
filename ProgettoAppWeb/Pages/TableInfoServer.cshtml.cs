using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProgettoAppWeb.Data;
using ProgettoAppWeb.Models;

namespace ProgettoAppWeb.Pages
{
    public class TableInfoServerModel : PageModel
    {
        private readonly ServerContext _context;

        public TableInfoServerModel(ServerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TableInfo table { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (_context.Database.CanConnect())
            {
                //assegna le info
                return Page();
            }
            else
            {
                //errore (puoi ritornare la pagina errore)
                return RedirectToPage("./Error");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _context.Database.CloseConnection();
            ConnectionStringServer.setConnectionString("", "", "", "");
            return RedirectToPage("./Index");
        }
    }
}
