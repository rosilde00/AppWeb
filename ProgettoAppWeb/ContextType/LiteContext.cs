using Microsoft.EntityFrameworkCore;
using ProgettoAppWeb.Models;

namespace ProgettoAppWeb.Data
{
    public class LiteContext : DbContext
    {
        public LiteContext(DbContextOptions<LiteContext> options)
        : base(options)
        { }
        public DbSet<SQLiteModel>? SQLiteModel { get; set; }
    }

    public static class ConnectionStringLite
    {
        public static string connectionString = "Data Source=";

        public static void setConnectionString (string path)
        {
            connectionString = $"Data Source={path}";
        }
    }
}
