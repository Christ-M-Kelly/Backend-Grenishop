using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackendGrenishop.DTOs.Request;

public class CreateProduitDto
{
    [Required(ErrorMessage = "Le nom du produit est obligatoire")]
    [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
    [JsonPropertyName("nom")]
    public required string Nom { get; set; }

    [Required(ErrorMessage = "L'état du produit est obligatoire")]
    [RegularExpression("^(Neuf|Occasion)$", ErrorMessage = "L'état doit être 'Neuf' ou 'Occasion'")]
    [JsonPropertyName("etat")]
    public required string Etat { get; set; }

    [Required(ErrorMessage = "L'ID du modèle est obligatoire")]
    [Range(1, int.MaxValue, ErrorMessage = "L'ID du modèle doit être supérieur à 0")]
    [JsonPropertyName("id_modele")]
    public int IdModele { get; set; }
}
