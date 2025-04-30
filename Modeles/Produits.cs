using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendGrenishop.Modeles;

public class Produit
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id_produit { get; set; }

    [Required]
    [StringLength(100)]
    [Column(TypeName = "varchar(100)")]
    public required string Nom { get; set; }

    [Required]
    [StringLength(50)]
    [Column(TypeName = "varchar(50)")]
    public required string Etat { get; set; }

    [ForeignKey("Commande")]
    public int? id_commande { get; set; }

    [ForeignKey("Modele")]
    public int id_modele { get; set; }

    // Navigation properties
    [JsonIgnore]
    public virtual Commande? Commande { get; set; }
    [JsonIgnore]
    public virtual Modele? Modele { get; set; }
} 