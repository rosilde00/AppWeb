using System.ComponentModel.DataAnnotations;

namespace ProgettoAppWeb.Models
{
    public class SQLServerModel
    {
        [Required]
        [Display(Name = "Indirizzo del server")]
        public string serverAddress { get; set; }

        [Required]
        [Display(Name = "Nome del database")]
        public string db { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string username { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string password { get; set; }
    }
}
