using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendGrenishop.Modeles;

public class ListeDeSouhaits
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id_liste { get; set; }

    [ForeignKey("Modele")]
    public required int id_modele { get; set; }

    [ForeignKey("Compte")]
    public required int id_compte { get; set; }

    // Navigation properties
    [JsonIgnore]
    public virtual Modele? Modele { get; set; }
    [JsonIgnore]
    public virtual Compte? Compte { get; set; }
} 