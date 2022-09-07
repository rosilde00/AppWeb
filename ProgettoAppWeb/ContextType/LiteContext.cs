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

    /**
     * Classe che contiene la lista delle connection string SQLite corrispondenti alle connessioni
     * aperte fino ad ora
     * **/
    public static class ConnectionStringLite
    {
        public static LinkedList<string> listConnectionStrings = new LinkedList<string> (new[] {"empty"});

        public static void addConnectionString (string path)
        {
            listConnectionStrings.AddLast($"Data Source={path}");
        }

        /**
         * Rimuove l'ultima connection string
         * **/
        public static void deleteConnectionString()
        {
            listConnectionStrings.RemoveLast();
        }
    }
}
