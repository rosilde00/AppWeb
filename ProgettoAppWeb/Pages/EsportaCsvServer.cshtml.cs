using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProgettoAppWeb.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using ProgettoAppWeb.Tools;
using System.ComponentModel.DataAnnotations;

namespace ProgettoAppWeb.Pages
{
    public class EsportaCsvServerModel : PageModel
    {
        public readonly ServerContext _context;
        public readonly ILogger<EsportaCsvServerModel> _logger;
        public EsportaCsvServerModel(ServerContext context, ILogger<EsportaCsvServerModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        [Display(Name = "Percorso del file")]
        public string filePath { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            LinkedList<string> tabelle = getTables();
            DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();

            string csv = string.Empty;

            foreach (string s in tabelle)
            {
                cmd.CommandText = $"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = \'{s}\'";
                DbDataReader res = cmd.ExecuteReader();
                while (res.Read())
                {
                    csv += res[3].ToString() + ',';
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
                    return RedirectToPage("./Errors/ErrorFile");
                }
                csv = string.Empty;
            }

            return RedirectToPage("./TableInfoServer");
        }

        private LinkedList<string> getTables()
        {
            LinkedList<string> tabelle = new LinkedList<string>();
            DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();
            _context.Database.GetDbConnection().Open();
            cmd.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES";
            DbDataReader res = cmd.ExecuteReader();
            while (res.Read())
            {
                tabelle.AddLast(res[0].ToString());
            }
            res.Close();
            cmd.Dispose();
            return tabelle;
        }
    }
}