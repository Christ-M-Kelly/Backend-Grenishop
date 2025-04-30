using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendGrenishop.Modeles
{
    public class Compte
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_compte { get; set; }
        
        [Required]
        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public required string Nom { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public required string Prenom { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public required string Email { get; set; }
        
        [Required]
        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        [JsonIgnore]
        public required string MotDePasse { get; set; }
        
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime date_inscription { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [JsonIgnore]
        public virtual ICollection<Commande>? Commandes { get; set; }
        [JsonIgnore]
        public virtual ICollection<ListeDeSouhaits>? ListeDeSouhaits { get; set; }
    }
}
