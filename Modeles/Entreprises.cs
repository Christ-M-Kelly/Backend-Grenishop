using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendGrenishop.Modeles;

public class Entreprise
{
    [Key]
    public int EntrepriseID { get; set; }
    
    [Required]
    [StringLength(100)]
    public required string Nom_Entreprise { get; set; }

    // Navigation property
    public virtual ICollection<Produit>? Produits { get; set; }
}
