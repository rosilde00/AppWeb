using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProgettoAppWeb.Data;
using ProgettoAppWeb.Models;

namespace ProgettoAppWeb.Pages
{
    public class TableInfoLiteModel : PageModel
    {
        private readonly LiteContext _context;
        private readonly ILogger<TableInfoLiteModel> _logger;


        public TableInfoLiteModel(LiteContext context, ILogger<TableInfoLiteModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public Tables tables { get; set; } = default!;

        /**
         * Ottiene tutte le tabelle e informazioni dal DB
         * che devono essere stampate a schermo
         **/
        public async Task<IActionResult> OnGetAsync()
        {
            if (_context.Database.CanConnect())
            {
                //raccoglie i nomi di tutte le tabelle nel DB
                DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();
                _context.Database.GetDbConnection().Open();
                cmd.CommandText = "SELECT name FROM sqlite_schema WHERE type='table'";
                DbDataReader reader = cmd.ExecuteReader();

                tables=new Tables();

                //per ogni tabella esegue le operazioni
                while (reader.Read())
                {
                    var table = new TableInfo
                    {
                        Name = reader[0].ToString()
                    };

                    //ottiene le informazioni sulle colonne
                    DbCommand cmd2 = _context.Database.GetDbConnection().CreateCommand();
                    cmd2.CommandText = $"PRAGMA table_info({reader[0]})";
                    DbDataReader reader2 = cmd2.ExecuteReader();

                    if (reader2 != null)
                    {
                        while (reader2.Read())
                        {
                            var notnull = "NULL";
                            if (reader2[3].ToString().Equals("1"))
                            {
                                notnull = "NOT NULL";
                            }

                            table.setColumn(reader2[1].ToString(), reader2[2].ToString(), notnull);

                            //ottiene le chiavi primarie
                            if (!reader2[5].ToString().Equals("0"))
                            {
                                table.addPk(reader2[1].ToString());
                            }
                        }

                        reader2.Close();
                    }

                    //ottiene le chiavi esterne
                    cmd2.CommandText = $"PRAGMA foreign_key_list({reader[0]})";
                    reader2 = cmd2.ExecuteReader();

                    if (reader2 != null)
                    {
                        while (reader2.Read())
                        {
                            table.addFKey(reader2[2].ToString(), reader2[3].ToString(), reader2[4].ToString());
                        }

                        reader2.Close();
                    }

                    //ottiene la lista degli indici
                    cmd2.CommandText = $"PRAGMA index_list('{reader[0]}')";
                    reader2 = cmd2.ExecuteReader();

                    if (reader2 != null)
                    {
                        //per ogni indice nella lista
                        while (reader2.Read())
                        {
                            //ottiene le informazioni dell'indice
                            DbCommand cmd3 = _context.Database.GetDbConnection().CreateCommand();
                            cmd3.CommandText = $"PRAGMA index_info('{reader2[1]}')";
                            DbDataReader reader3 = cmd3.ExecuteReader();
                            if(reader3 != null)
                            {
                                LinkedList<string> cols = new LinkedList<string>();
                                while (reader3.Read())
                                {
                                    cols.AddLast(reader3[2].ToString());
                                }

                                table.addIndex(reader2[1].ToString(), cols);
                            }
                        }

                        reader2.Close();
                    }

                    tables.addTable(table);
                }

                reader.Close();
                _context.Database.GetDbConnection().Close();
                return Page();
            }
            else
            {
                return RedirectToPage("./Errors/ErrorConnection");
            }
        }

        public async Task<IActionResult> OnPostAsync ()
        {
            _context.Database.CloseConnection();
            ConnectionStringLite.deleteConnectionString();
            return RedirectToPage("./Index");
        }
    }
}
