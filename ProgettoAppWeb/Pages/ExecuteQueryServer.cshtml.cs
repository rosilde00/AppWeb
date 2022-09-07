using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProgettoAppWeb.Data;
using ProgettoAppWeb.Models;

namespace ProgettoAppWeb.Pages
{
    public class ExecuteQueryServerModel : PageModel
    {
        private readonly ServerContext _context;
        private readonly ILogger<ExecuteQueryServerModel> _logger;

        [BindProperty]
        public string Query { get; set; } = default!;
        public QueryResult FieldsValues { get; set; } = default!;

        public ExecuteQueryServerModel(ServerContext context, ILogger<ExecuteQueryServerModel> logger)
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
                FieldsValues.Status = null;
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

                                reader.Close();
                                cmd.CommandText = $"EXEC sp_columns '{querySplit[3]}'";
                                reader=cmd.ExecuteReader();
                                LinkedList<string> list2 = new LinkedList<string>();
                                while (reader.Read())
                                {
                                    list2.AddLast(reader[3].ToString());
                                }

                                FieldsValues.addFirstRow(0, list2);
                                reader.Close();
                            }
                            else
                            {
                                FieldsValues.Status = "NO VALUES FOUND";
                            }
                        }
                        catch (SqlException e)
                        {
                            FieldsValues.Status = "ERROR: " + e.Message;
                        }
                        break;

                    case "INSERT":
                        cmd.CommandText = Query;
                        try
                        {
                            DbDataReader reader = cmd.ExecuteReader();
                            if (reader != null)
                            {
                                reader.Close();
                                cmd.CommandText = "SELECT @@ROWCOUNT";
                                reader = cmd.ExecuteReader();
                                if (reader != null)
                                {
                                    reader.Read();
                                    FieldsValues.Status = $"{reader[0]} row/s added successfully";
                                    reader.Close();
                                }

                                cmd.CommandText = "SELECT * FROM " + querySplit[2].Split("(")[0];
                                reader = cmd.ExecuteReader();

                                for (int k = 1; reader.Read(); k++)
                                {
                                    LinkedList<string> list = new LinkedList<string>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        list.AddLast(reader[i].ToString());
                                    }

                                    FieldsValues.addRow(k, list);
                                }

                                reader.Close();
                                cmd.CommandText = $"EXEC sp_columns '{querySplit[2].Split("(")[0]}'";
                                reader = cmd.ExecuteReader();
                                LinkedList<string> list2 = new LinkedList<string>();
                                while (reader.Read())
                                {
                                    list2.AddLast(reader[3].ToString());
                                }

                                FieldsValues.addFirstRow(0, list2);
                                reader.Close();
                            }
                        }
                        catch (SqlException e)
                        {
                            FieldsValues.Status = "ERROR: " + e.Message;
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
                                cmd.CommandText = "SELECT @@ROWCOUNT";
                                reader = cmd.ExecuteReader();
                                if (reader != null)
                                {
                                    reader.Read();
                                    FieldsValues.Status = $"{reader[0]} row/s updated successfully";
                                    reader.Close();
                                }

                                cmd.CommandText = "SELECT * FROM " + querySplit[1];
                                reader = cmd.ExecuteReader();

                                for (int k = 1; reader.Read(); k++)
                                {
                                    LinkedList<string> list = new LinkedList<string>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        list.AddLast(reader[i].ToString());
                                    }

                                    FieldsValues.addRow(k, list);
                                }

                                reader.Close();
                                cmd.CommandText = $"EXEC sp_columns '{querySplit[1]}'";
                                reader = cmd.ExecuteReader();
                                LinkedList<string> list2 = new LinkedList<string>();
                                while (reader.Read())
                                {
                                    list2.AddLast(reader[3].ToString());
                                }

                                FieldsValues.addFirstRow(0, list2);
                                reader.Close();
                            }
                        }
                        catch (SqlException e)
                        {
                            FieldsValues.Status = "ERROR: " + e.Message;
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
                                cmd.CommandText = "SELECT @@ROWCOUNT";
                                reader = cmd.ExecuteReader();
                                if (reader != null)
                                {
                                    reader.Read();
                                    FieldsValues.Status = $"{reader[0]} row/s deleted successfully";
                                    reader.Close();
                                }

                                cmd.CommandText = "SELECT * FROM " + querySplit[2];
                                reader = cmd.ExecuteReader();

                                for (int k = 1; reader.Read(); k++)
                                {
                                    LinkedList<string> list = new LinkedList<string>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        list.AddLast(reader[i].ToString());
                                    }

                                    FieldsValues.addRow(k, list);
                                }

                                reader.Close();
                                cmd.CommandText = $"EXEC sp_columns '{querySplit[2]}'";
                                reader = cmd.ExecuteReader();
                                LinkedList<string> list2 = new LinkedList<string>();
                                while (reader.Read())
                                {
                                    list2.AddLast(reader[3].ToString());
                                }

                                FieldsValues.addFirstRow(0, list2);
                                reader.Close();
                            }
                        }
                        catch (SqlException e)
                        {
                            FieldsValues.Status = "ERROR: "+e.Message;
                        }
                        break;

                    default:
                        FieldsValues.Status = $"ERROR: unknown statement: '{querySplit[0]}'";
                        break;
                }

                return Page();
            }
            else
            {
                return RedirectToPage("./Errors/ErrorConnection");
            }
        }
    }
}
