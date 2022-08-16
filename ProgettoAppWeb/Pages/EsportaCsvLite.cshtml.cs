using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProgettoAppWeb.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using ProgettoAppWeb.Tools;

namespace ProgettoAppWeb.Pages
{
    public class EsportaCsvModel : PageModel
    {

        public readonly LiteContext _context;
        private readonly ILogger<EsportaCsvModel> _logger;
        public EsportaCsvModel(LiteContext context, ILogger<EsportaCsvModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public string filePath { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            LinkedList<string> tabelle = getTables();

            DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();
            _context.Database.GetDbConnection().Open();

            string csv = string.Empty;

            foreach (string s in tabelle)
            {
                cmd.CommandText = $"PRAGMA table_info({s});";
                DbDataReader res = cmd.ExecuteReader();
                while (res.Read())
                {   
                    csv += res[1].ToString() + ',';
                }
                res.Close();
                csv = csv.Substring(0, csv.Length - 1);
                csv += "\r\n";

                cmd.CommandText = $"SELECT * FROM {s}";
                DbDataReader results = cmd.ExecuteReader();
                while (results.Read())
                {
                    for (int i = 0; i < results.FieldCount; i++)
                    {
                        csv += results[i].ToString() + ',';
                    }
                    csv = csv.Substring(0, csv.Length - 1);
                    csv += "\r\n";
                }
                results.Close();

                bool done = FileWriter.WriteCsv(filePath, s, csv);
                if (!done)
                {
                    return RedirectToPage("./Error");
                }
                csv = string.Empty;
            }

            return RedirectToPage("./TableInfoLite");
        }

        private LinkedList<string> getTables()
        {
            LinkedList<string> tabelle = new LinkedList<string>();
            DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();
            _context.Database.GetDbConnection().Open();
            cmd.CommandText = "SELECT name FROM sqlite_schema WHERE type='table'";
            DbDataReader res = cmd.ExecuteReader();
            while (res.Read())
            {
                if (!res[0].ToString().Equals("sqlite_sequence"))
                    tabelle.AddLast(res[0].ToString());
            }
            res.Close();
            cmd.Dispose();
            return tabelle;
        }

    }
}
