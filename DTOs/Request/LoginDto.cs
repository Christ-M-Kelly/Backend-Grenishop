using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackendGrenishop.DTOs.Request;

public class LoginDto
{
    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    [JsonPropertyName("motDePasse")]
    public required string MotDePasse { get; set; }
}
