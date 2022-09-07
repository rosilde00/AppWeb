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

        /**
         * Ottiene il contenuto del file al path specificato, lo interpreta opportunamente
         * e esegue una o più query di insert (una per ogni riga di dati contenuta nel file)
         * **/
        public async Task<IActionResult> OnPostAsync()
        {
            string[] path = filePath.Split("/");
            string tabella = path[path.Length - 1].Remove(path[path.Length - 1].Length - 4);
            string[]? csv = FileReader.ReadCsv(filePath); //preleva il contenuto del file
            if (csv == null)
                return RedirectToPage("./Errors/ErrorFile");

            DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();
            _context.Database.GetDbConnection().Open();
            string colonne = csv[0]; //preleva la prima riga del file, contenente le colonne

            foreach (string line in csv)
            {
                if (!line.Equals(colonne)) //ottiene i valori da inserire nel database, scartando la prima riga
                {
                    LinkedList<string> listValue = new LinkedList<string>();
                    string[] values = line.Split(',');

                    foreach (string value in values)
                    {
                        if (!Regex.IsMatch(value, @"^\d+$")) //se il dato non è un numero, nella query va indicato con le virgolette
                            listValue.AddLast($"\"{value}\"");
                        else
                            listValue.AddLast(value);
                    }

                    //costruisce una stringa con tutti i valori da inserire separati da una virgola
                    string v = "";
                    foreach (string value in listValue)
                    {
                        v = v + value + ",";
                    }
                    v = v.Remove(v.Length - 1);

                    //esegue la query di insert
                    string sql = $"INSERT INTO {tabella} ({colonne}) VALUES({v});";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToPage("./TableInfoLite");
        }

    }
}
