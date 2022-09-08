namespace ProgettoAppWeb.Models
{
    /**
     * Classe che rappresenta il risultato di una query,
     * salva lo status della query che indica le righe modificate 
     * o eventuali errori avvenuti nella query
     **/
    public class QueryResult
    {
        public LinkedList<Row> Rows { get; set; }
        public Row CurrentRow { get; set; }
        public string Status { get; set; }

        public QueryResult()
        {
            Rows = new LinkedList<Row>();
        }

        public void addRow(int id, LinkedList<string> list)
        {
            Row row = new Row(id,list);
            Rows.AddLast(row);
        }

        public void addFirstRow(int id,LinkedList<string> list)
        {
            Row row = new(id, list);
            Rows.AddFirst(row);
        }

        public int getRows()
        {
            return Rows.Count;
        }

        public int getColumns()
        {
            return Rows.First.Value.getCount();
        }

        public void setCurrentRow(int id)
        {
            foreach (Row row in Rows)
            {
                if (row.Id.Equals(id))
                {
                    CurrentRow = row;
                }
            }
        }

        public string selectValue(int index)
        {
            return CurrentRow.Values.ElementAt(index);
        }

        public void clear()
        {
            foreach (Row row in Rows)
            {
                row.clear();
            }
            Rows.Clear();
        }

        /**
         * Classe che rappresenta le righe della tabella
         * utilizzata per mostrare risultati di modifiche applicate ad essa
         * e come output di query di SELECT
         **/
        public class Row
        {
            public int Id { get; set; }
            public LinkedList<string> Values { get; set; }

            public Row(int id, LinkedList<string> list)
            {
                Id= id;
                Values = list;
            }

            public int getCount()
            {
                return Values.Count;
            }

            public void clear()
            {
                Values.Clear();
            }
        }
    }
}
