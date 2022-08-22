namespace ProgettoAppWeb.Models
{
    public class Tables
    {
        public LinkedList<TableInfo> TablesInfo { get; set; }

        public Tables()
        {
            TablesInfo = new LinkedList<TableInfo>();
        }

        public void addTable(TableInfo tab)
        {
            TablesInfo.AddLast(tab);
        }
    }
}
