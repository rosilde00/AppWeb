using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProgettoAppWeb.Data;
using ProgettoAppWeb.Tools;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace ProgettoAppWeb.Pages
{
    public class ImportaCsvLiteModel : PageModel
    {
        public readonly LiteContext _context;
        private readonly ILogger<ImportaCsvLiteModel> _logger;
        public ImportaCsvLiteModel(LiteContext context, ILogger<ImportaCsvLiteModel> logger)
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
            string[] path = filePath.Split("/");
            string tabella = path[path.Length - 1].Remove(path[path.Length - 1].Length - 4);
            string[]? csv = FileReader.ReadCsv(filePath);
            if (csv == null)
                return RedirectToPage("./Errors/ErrorFile");

            DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();
            _context.Database.GetDbConnection().Open();
            string colonne = csv[0];

            foreach (string line in csv)
            {
                if (!line.Equals(colonne))
                {
                    LinkedList<string> listValue = new LinkedList<string>();
                    string[] values = line.Split(',');

                    foreach (string value in values)
                    {
                        if (!Regex.IsMatch(value, @"^\d+$"))
                            listValue.AddLast($"\"{value}\"");
                        else
                            listValue.AddLast(value);
                    }

                    string v = "";
                    foreach (string value in listValue)
                    {
                        v = v + value + ",";
                    }
                    v = v.Remove(v.Length - 1);

                    string sql = $"INSERT INTO {tabella} ({colonne}) VALUES({v});";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToPage("./TableInfoLite");
        }

    }
}
