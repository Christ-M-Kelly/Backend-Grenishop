using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendGrenishop.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(100)]
    [Column(TypeName = "varchar(100)")]
    public required string Nom { get; set; }

    [Required]
    [StringLength(100)]
    [Column(TypeName = "varchar(100)")]
    public required string Prenom { get; set; }

    [Required]
    public DateTime DateInscription { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<Modeles.Commande>? Commandes { get; set; }
    public virtual ICollection<Modeles.ListeDeSouhaits>? ListeDeSouhaits { get; set; }
}
