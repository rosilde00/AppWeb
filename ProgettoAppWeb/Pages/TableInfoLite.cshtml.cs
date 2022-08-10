using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProgettoAppWeb.Data;
using ProgettoAppWeb.Models;

namespace ProgettoAppWeb.Pages
{
    //qui mostri le info del db
    public class TableInfoLiteModel : PageModel
    {
        private readonly LiteContext _context;
        private readonly ILogger<AddSQLiteModel> _logger;

        public TableInfoLiteModel(LiteContext context, ILogger<AddSQLiteModel> logger)
        {
            _context = context;
            _logger = logger;
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

        public async Task<IActionResult> OnPostAsync ()
        {
            _context.Database.CloseConnection();
            ConnectionStringLite.setConnectionString("");
            return RedirectToPage("./Index");
        }
    }
}
