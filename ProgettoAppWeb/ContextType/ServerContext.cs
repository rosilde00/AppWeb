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

    public static class ConnectionStringServer
    {
        public static LinkedList<string> listConnectionStrings = new LinkedList<string>(new[] { "empty" });
        public static int actual = 0;

        public static void addConnectionString(string server, string db, string username, string password)
        {
            listConnectionStrings.AddLast($"Server = {server}; Database = {db}; User Id = {username}; Password = {password};");
            actual = listConnectionStrings.Count - 1;
        }

        public static void addConnectionStringLocal(string server, string db)
        {
            listConnectionStrings.AddLast($"Server = {server}; Database = {db}; Trusted_Connection=True;");
            actual = listConnectionStrings.Count - 1;
        }

        public static void deleteConnectionString()
        {
            listConnectionStrings.RemoveLast();
            actual = listConnectionStrings.Count - 1;
        }

    }
}
