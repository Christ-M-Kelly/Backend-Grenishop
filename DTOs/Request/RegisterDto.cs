using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackendGrenishop.DTOs.Request;

public class RegisterDto
{
    [Required(ErrorMessage = "Le nom est obligatoire")]
    [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
    [JsonPropertyName("nom")]
    public required string Nom { get; set; }

    [Required(ErrorMessage = "Le prénom est obligatoire")]
    [StringLength(100, ErrorMessage = "Le prénom ne peut pas dépasser 100 caractères")]
    [JsonPropertyName("prenom")]
    public required string Prenom { get; set; }

    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
    [StringLength(100, ErrorMessage = "L'email ne peut pas dépasser 100 caractères")]
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir entre 6 et 100 caractères")]
    [JsonPropertyName("motDePasse")]
    public required string MotDePasse { get; set; }

    [Required(ErrorMessage = "La confirmation du mot de passe est obligatoire")]
    [Compare("MotDePasse", ErrorMessage = "Les mots de passe ne correspondent pas")]
    [JsonPropertyName("confirmMotDePasse")]
    public required string ConfirmMotDePasse { get; set; }
}
