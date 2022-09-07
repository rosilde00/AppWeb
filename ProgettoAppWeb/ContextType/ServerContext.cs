using Microsoft.EntityFrameworkCore;
using ProgettoAppWeb.Models;

namespace ProgettoAppWeb.Data
{
    public class ServerContext : DbContext
    {
        public ServerContext(DbContextOptions<ServerContext> options)
       : base(options)
        { }
    }

    /**
     * Classe che contiene la lista delle connection string SQLServer corrispondenti alle connessioni
     * aperte fino ad ora
     * **/
    public static class ConnectionStringServer
    {
        public static LinkedList<string> listConnectionStrings = new LinkedList<string>(new[] { "empty" });

        /**
         * Aggiunge una connection string con nome utente e password
         * **/
        public static void addConnectionString(string server, string db, string username, string password)
        {
            listConnectionStrings.AddLast($"Server = {server}; Database = {db}; User Id = {username}; Password = {password};");
        }

        /**
         * Aggiunge una connection string con trusted connection
         * **/
        public static void addConnectionStringLocal(string server, string db)
        {
            listConnectionStrings.AddLast($"Server = {server}; Database = {db}; Trusted_Connection=True;");
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
