namespace ProgettoAppWeb.Models
{
    /**
     * Classe che rappresenta una signola tabella del DB,
     * contiene tutte le informazioni da mostrare a schermo
     **/
    public class TableInfo
    {
        public string Name { get; set; }
        public LinkedList<Colonna> Columns { get; set; }
        public LinkedList<string> PkColumns { get; set; }
        public LinkedList<Index> Indexes { get; set; }
        public LinkedList<Foreign_key> Foreign_Keys { get; set; }

        public TableInfo()
        {
            Name = "";
            Columns = new LinkedList<Colonna>();
            PkColumns = new LinkedList<string>();
            Foreign_Keys = new LinkedList<Foreign_key>();
            Indexes = new LinkedList<Index>();
        }

        /**
         * Crea una colonna e la aggiunge alla lista 
         **/
        public void setColumn(string name, string type, string isnull)
        {
            Colonna col = new()
            {
                Name = name,
                dataType = type,
                isNull = isnull
            };

            Columns.AddLast(col);
        }

        /**
         * Aggiunge una chiave primaria alla lista
         **/
        public void addPk(string pk)
        {
            PkColumns.AddLast(pk);
        }

        /**
         * Crea un indice e lo aggiunge alla lista 
         **/
        public void addIndex(string name, Object cols)
        {
            Index ind;
            if (cols is string)
            {
                ind = new Index(name, (string) cols); //indice per SQLServer
            }
            else
            {
                ind = new Index(name, (LinkedList<string>) cols); //indice per SQLite
            }

            Indexes.AddLast(ind);
        }

        /**
         * Crea una chiave esterna e la aggiunge alla lista
         **/
        public void addFKey(string table, string from, string to)
        {
            Foreign_key fk = new()
            {
                Table = table,
                From = from,
                To = to
            };

            Foreign_Keys.AddLast(fk);
        }

        /**
         * Classe che rappresenta le colonne della tabella
         **/
        public class Colonna
        {
            public string Name { get; set; }
            public string dataType { get; set; }
            public string isNull { get; set; }
        }

        /**
         * Classe che rappresenta le chiavi esterne della tabella
         **/
        public class Foreign_key
        {
            public string Table { get; set; }
            public string From { get; set; }
            public string To { get; set; }
        }

        /**
         * Classe che rappresenta gli indici definiti sulla tabella
         **/
        public class Index
        {
            public string Name { get; set; }
            public LinkedList<string> Cols { get; set; }

            public Index(string name, LinkedList<string> cols)
            {
                Name = name;
                Cols = cols;
            }

            public Index(string name, string cols)
            {
                LinkedList<string> col=new LinkedList<string>();
                
                foreach(string str in cols.Split(","))
                {
                    col.AddLast(str.Trim());
                }

                Name= name;
                Cols = col;
            }
        }
    }
}
