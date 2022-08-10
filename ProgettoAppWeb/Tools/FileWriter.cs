using System.IO;

namespace ProgettoAppWeb.Tools
{
    public static class FileWriter
    {
        public static bool WriteCsv (string path, string tableName, string csv)
        {
            try
            {
                File.WriteAllText($"{path}/{tableName}.csv", csv);
                return true;
            }
            catch (IOException e)
            {
                return false;
            }
            
        }
    }
}
