using System.Text.Json.Serialization;

namespace BackendGrenishop.DTOs.Response;

public class UserProfileDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("nom")]
    public string Nom { get; set; } = string.Empty;

    [JsonPropertyName("prenom")]
    public string Prenom { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("date_inscription")]
    public DateTime DateInscription { get; set; }
}
