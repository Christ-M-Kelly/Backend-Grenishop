using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendGrenishop.Modeles;

public class Commande
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id_commande { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime date_commande { get; set; }

    public DateTime? date_reception { get; set; }

    [Required]
    [StringLength(50)]
    [Column(TypeName = "varchar(50)")]
    public required string status_commande { get; set; }

    [Required]
    [StringLength(200)]
    [Column(TypeName = "varchar(200)")]
    public required string adresse_livraison { get; set; }

    [ForeignKey("Compte")]
    public int id_compte { get; set; }

    // Navigation property
    [JsonIgnore]
    public virtual Compte? Compte { get; set; }
    [JsonIgnore]
    public virtual ICollection<Produit>? Produits { get; set; }
} 