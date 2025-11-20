using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BackendGrenishop.Models;

namespace BackendGrenishop.Modeles;

public class Commande
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id_commande { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime date_commande { get; set; } = DateTime.UtcNow;

    public DateTime? date_reception { get; set; }

    [Required]
    [StringLength(50)]
    [Column(TypeName = "varchar(50)")]
    [RegularExpression("^(En attente|En cours|Livrée|Annulée)$", 
        ErrorMessage = "Le statut doit être: En attente, En cours, Livrée ou Annulée")]
    public required string status_commande { get; set; }

    [Required]
    [StringLength(200)]
    [Column(TypeName = "varchar(200)")]
    public required string adresse_livraison { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public required decimal prix_total { get; set; }

    // Foreign key for ApplicationUser (Identity)
    [Required]
    public required string UserId { get; set; }

    // Navigation properties
    [JsonIgnore]
    [ForeignKey("UserId")]
    public virtual ApplicationUser? ApplicationUser { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<Produit>? Produits { get; set; }
}
 