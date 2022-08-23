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
    public class TableInfoServerModel : PageModel
    {
        private readonly ServerContext _context;

        public TableInfoServerModel(ServerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Tables tables { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (_context.Database.CanConnect())
            {
                LinkedList<string> names = new LinkedList<string>();

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

                foreach (string name in names)
                {
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
                //errore (puoi ritornare la pagina errore)
                return RedirectToPage("./Error");
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
