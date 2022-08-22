using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IActionResult> OnGetAsync()
        {
            if (_context.Database.CanConnect())
            {
                DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();
                _context.Database.GetDbConnection().Open();
                cmd.CommandText = "SELECT name FROM sqlite_schema WHERE type='table'";
                DbDataReader reader = cmd.ExecuteReader();

                tables=new Tables();

                while (reader.Read())
                {
                    var table = new TableInfo
                    {
                        Name = reader[0].ToString()
                    };

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

                            if (!reader2[5].ToString().Equals("0"))
                            {
                                table.addPk(reader2[1].ToString());
                            }
                        }

                        reader2.Close();
                    }

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

                    cmd2.CommandText = $"PRAGMA index_list('{reader[0]}')";
                    reader2 = cmd2.ExecuteReader();

                    if (reader2 != null)
                    {
                        while (reader2.Read())
                        {
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
                tables.TablesInfo.RemoveFirst();
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
