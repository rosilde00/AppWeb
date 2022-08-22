using System.ComponentModel.DataAnnotations;

namespace ProgettoAppWeb.Models
{
    public class SQLServerModel
    {
        [Required]
        [Display(Name = "Indirizzo del server")]
        public string serverAddress { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Nome del database")]
        public string db { get; set; } = string.Empty;

        [Display(Name = "Username")]
        public string username { get; set; } = string.Empty;

        [Display(Name = "Password")]
        public string password { get; set; } = string.Empty;
    }
}
