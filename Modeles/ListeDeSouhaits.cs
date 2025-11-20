using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BackendGrenishop.Models;

namespace BackendGrenishop.Modeles;

public class ListeDeSouhaits
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id_liste { get; set; }

    [ForeignKey("Modele")]
    public required int id_modele { get; set; }

    // Foreign key for ApplicationUser (Identity)
    [Required]
    public required string UserId { get; set; }

    // Navigation properties
    [JsonIgnore]
    public virtual Modele? Modele { get; set; }
    
    [JsonIgnore]
    [ForeignKey("UserId")]
    public virtual ApplicationUser? ApplicationUser { get; set; }
}
 