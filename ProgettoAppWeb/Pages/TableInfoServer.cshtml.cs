using System.Data.Common;
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
        public Tables tables { get;set; } = default!;

        /**
         * Ottiene tutte le tabelle e informazioni dal DB
         * che devono essere stampate a schermo
         **/ 
        public async Task<IActionResult> OnGetAsync()
        {
            if (_context.Database.CanConnect())
            {
                LinkedList<string> names = new LinkedList<string>();

                //raccoglie i nomi di tutte le tabelle nel DB
                DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();
                _context.Database.GetDbConnection().Open();
                cmd.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES";
                DbDataReader reader = cmd.ExecuteReader();

                tables = new Tables();

                while (reader.Read())
                {
                    names.AddLast(reader[0].ToString());
                }

                reader.Close();

                //per ogni tabella esegue le operazioni
                foreach (string name in names)
                {
                    //ottiene le informazioni sulle colonne
                    cmd.CommandText = $"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME= '{name}'";
                    reader = cmd.ExecuteReader();

                    var table = new TableInfo();
                    table.Name = name;

                    while (reader.Read())
                    {
                        var notnull = "NULL";
                        if (reader[6].ToString().Equals("NO"))
                        {
                            notnull = "NOT NULL";
                        }

                        table.setColumn(reader[3].ToString(), reader[7].ToString(), notnull);
                    }

                    reader.Close();

                    //ottiene le chiavi primarie 
                    cmd.CommandText = $"EXEC sp_pkeys '{name}'";
                    reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            table.PkColumns.AddLast(reader[3].ToString());
                        }
                        reader.Close();
                    }

                    reader.Close();

                    //ottiene le chiavi esterne
                    cmd.CommandText = $"EXEC sp_fkeys '{name}'";
                    reader=cmd.ExecuteReader();
                    if(reader != null)
                    {
                        while (reader.Read())
                        {
                            table.addFKey(reader[6].ToString(), reader[7].ToString(), reader[3].ToString());
                        }
                    }

                    reader.Close();

                    //ottiene gli indici
                    cmd.CommandText = $"EXEC sp_helpindex '{name}'";
                    reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            table.addIndex(reader[0].ToString(), reader[2].ToString());
                        }
                    }

                    reader.Close();
                    tables.addTable(table);
                }

                _context.Database.GetDbConnection().Close();
                return Page();
            }
            else
            {
                return RedirectToPage("./Errors/ErrorConnection");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _context.Database.CloseConnection();
            ConnectionStringServer.deleteConnectionString();
            return RedirectToPage("./Index");
        }
    }
}
