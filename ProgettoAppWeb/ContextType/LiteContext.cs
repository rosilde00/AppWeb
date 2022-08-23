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
        public static LinkedList<string> listConnectionStrings = new LinkedList<string> (new[] {"empty"});
        public static int actual = 0;

        public static void addConnectionString (string path)
        {
            listConnectionStrings.AddLast($"Data Source={path}");
            actual = listConnectionStrings.Count-1;
        }

        public static void deleteConnectionString()
        {
            listConnectionStrings.RemoveLast();
            actual = listConnectionStrings.Count - 1;
        }
    }
}
