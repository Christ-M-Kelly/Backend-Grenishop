using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendGrenishop.Modeles;

public class Marque
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id_marque { get; set; }
    
    [Required]
    [StringLength(100)]
    [Column(TypeName = "varchar(100)")]
    public required string Nom { get; set; }

    // Navigation property
    [JsonIgnore]
    public virtual ICollection<Modele>? Modeles { get; set; }
}
