using System.ComponentModel.DataAnnotations;

namespace ProgettoAppWeb.Models
{
    public class SQLiteModel
    {
        [Required]
        [Display(Name = "Percorso")]
        public string path { get; set; }
    }
}
