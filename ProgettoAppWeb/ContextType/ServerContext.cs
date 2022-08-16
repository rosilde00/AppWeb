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

        public static string connectionString = "Server =; Database =; User Id =; Password =;";
        public static string connectionStringLocal = "Server=; Database=; Trusted_Connection=True;";

        public static void setConnectionString(string server, string db, string username, string password)
        {
            connectionString = $"Server = {server}; Database = {db}; User Id = {username}; Password = {password};";
        }

        public static void setConnectionStringLocal(string server, string db)
        {
            connectionString = $"Server = {server}; Database = {db}; Trusted_Connection=True;";
        }
    }
}
