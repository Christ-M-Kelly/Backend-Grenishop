using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendGrenishop.Modeles;

public class Produit
{
    [Key]
    public int ProduitID { get; set; }

    [Required]
    [StringLength(100)]
    public required string Nom { get; set; }

    [Required]
    [StringLength(50)]
    public required string Etat { get; set; }

    [ForeignKey("Entreprise")]
    public int EntrepriseID { get; set; }

    [Range(0, int.MaxValue)]
    public int Nombre_Neuf { get; set; }

    [Range(0, int.MaxValue)]
    public int Nombre_Occasion { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Range(0, double.MaxValue)]
    public decimal Prix_Neuf { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Range(0, double.MaxValue)]
    public decimal Prix_Occasion { get; set; }

    // Navigation property
    public virtual Entreprise? Entreprise { get; set; }
}
