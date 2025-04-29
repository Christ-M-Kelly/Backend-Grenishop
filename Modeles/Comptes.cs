using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendGrenishop.Modeles
{
    public class Compte
    {
        [Key]
        public int id_compte { get; set; }
        
        [Required]
        [StringLength(50)]
        public required string Nom { get; set; }

        [Required]
        [StringLength(50)]
        [JsonPropertyName("Prénom")]
        public required string Prenom { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public required string Email { get; set; }
        
        [Required]
        public required string MotDePasseHash { get; set; }
        
        [Required]
        public required DateTime Date_inscription { get; set; } = DateTime.UtcNow;
    }
}
