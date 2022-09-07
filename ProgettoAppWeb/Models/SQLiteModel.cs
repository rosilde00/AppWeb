using System.ComponentModel.DataAnnotations;

namespace ProgettoAppWeb.Models
{
    /**
     * Classe che contiene tutti i parametri necessari per costruire la connection
     * string di SQLite
     **/
    public class SQLiteModel
    {
        [Required]
        [Key]
        [Display(Name = "Percorso")]
        public string path { get; set; }
    }
}
