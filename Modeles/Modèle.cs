using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendGrenishop.Modeles;

public class Modele
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id_modele { get; set; }

    [Required]
    [StringLength(100)]
    [Column(TypeName = "varchar(100)")]
    public required string nom_modele { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public required int nbr_neuf { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public required int nbr_occasion { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0, double.MaxValue)]
    public required decimal prix_neuf { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0, double.MaxValue)]
    public required decimal prix_occasion { get; set; }

    [StringLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string? Tag { get; set; }

    [ForeignKey("Marque")]
    public int id_marque { get; set; }

    // Navigation properties
    [JsonIgnore]
    public virtual Marque? Marque { get; set; }
    [JsonIgnore]
    public virtual ICollection<Produit>? Produits { get; set; }
    [JsonIgnore]
    public virtual ICollection<ListeDeSouhaits>? ListeDeSouhaits { get; set; }
} 