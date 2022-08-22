using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProgettoAppWeb.Data;
using ProgettoAppWeb.Models;

namespace ProgettoAppWeb.Pages
{
    public class ExecuteQueryLiteModel : PageModel
    {
        private readonly LiteContext _context;
        private readonly ILogger<ExecuteQueryLiteModel> _logger;

        [BindProperty]
        public string Query { get; set; } = default!;
        public QueryResult FieldsValues { get; set; } = default!;

        public ExecuteQueryLiteModel(LiteContext context, ILogger<ExecuteQueryLiteModel> logger)
        {
            _context = context;
            _logger = logger;
            FieldsValues = new QueryResult();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (_context.Database.CanConnect())
            {
                string[] querySplit = this.Query.Split(" ");
                FieldsValues.clear();
                DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();
                _context.Database.GetDbConnection().Open();
                FieldsValues.Error = null;
                switch (querySplit[0])
                {
                    case "SELECT":
                        cmd.CommandText = Query;
                        try
                        {
                            DbDataReader reader = cmd.ExecuteReader();

                            if (reader.HasRows)
                            {
                                for (int k = 1; reader.Read(); k++)
                                {
                                    LinkedList<string> list = new LinkedList<string>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        list.AddLast(reader[i].ToString());
                                    }

                                    FieldsValues.addRow(k, list);
                                }

                                //-----------------------------------------------------
                                //STAMPA NOMI COLONNA
                                //-----------------------------------------------------
                                reader.Close();
                                cmd.CommandText = $"PRAGMA table_info({querySplit[3]})";
                                reader=cmd.ExecuteReader();
                                LinkedList<string> list2 = new LinkedList<string>();
                                while (reader.Read())
                                {
                                    list2.AddLast(reader[1].ToString());
                                }

                                FieldsValues.addFirstRow(0,list2);
                                reader.Close();
                                //-----------------------------------------------------
                            }
                            else
                            {
                                FieldsValues.Error = "NO VALUES FOUND";
                            }
                        }
                        catch (SqliteException e)
                        {
                            FieldsValues.Error = e.Message;
                        }
                        break;

                    case "INSERT":
                        cmd.CommandText = Query;
                        try
                        {
                            DbDataReader reader = cmd.ExecuteReader();
                            if(reader!= null)
                            {
                                reader.Close();
                                cmd.CommandText = "SELECT * FROM " + querySplit[2].Split("(")[0];
                                reader = cmd.ExecuteReader();

                                for (int k = 0; reader.Read(); k++)
                                {
                                    LinkedList<string> list = new LinkedList<string>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        list.AddLast(reader[i].ToString());
                                    }

                                    FieldsValues.addRow(k, list);
                                }
                            }
                        }
                        catch (SqliteException e)
                        {
                            FieldsValues.Error = e.Message;
                        }
                        break;

                    case "UPDATE":
                        cmd.CommandText = Query;
                        try
                        {
                            DbDataReader reader = cmd.ExecuteReader();
                            if (reader != null)
                            {
                                reader.Close();
                                cmd.CommandText = "SELECT * FROM " + querySplit[1];
                                reader = cmd.ExecuteReader();

                                for (int k = 0; reader.Read(); k++)
                                {
                                    LinkedList<string> list = new LinkedList<string>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        list.AddLast(reader[i].ToString());
                                    }

                                    FieldsValues.addRow(k, list);
                                }
                            }
                        }
                        catch (SqliteException e)
                        {
                            FieldsValues.Error = e.Message;
                        }
                        break;

                    case "DELETE":
                        cmd.CommandText = Query;
                        try
                        {
                            DbDataReader reader = cmd.ExecuteReader();
                            if (reader != null)
                            {
                                reader.Close();
                                cmd.CommandText = "SELECT * FROM " + querySplit[2];
                                reader = cmd.ExecuteReader();

                                for (int k = 0; reader.Read(); k++)
                                {
                                    LinkedList<string> list = new LinkedList<string>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        list.AddLast(reader[i].ToString());
                                    }

                                    FieldsValues.addRow(k, list);
                                }
                            }
                        }
                        catch (SqliteException e)
                        {
                            FieldsValues.Error = e.Message;
                        }
                        break;

                    default:
                        FieldsValues.Error = $"ERROR: unknown statement: '{querySplit[0]}'";
                        break;
                }

                return Page();
            }
            else
            {
                return RedirectToPage("./Error");
            }
        }
    }
}
