using System.Text.Json.Serialization;

namespace BackendGrenishop.DTOs.Response;

public class CommandeResponseDto
{
    [JsonPropertyName("id_commande")]
    public int IdCommande { get; set; }

    [JsonPropertyName("date_commande")]
    public DateTime DateCommande { get; set; }

    [JsonPropertyName("date_reception")]
    public DateTime? DateReception { get; set; }

    [JsonPropertyName("status_commande")]
    public string StatusCommande { get; set; } = string.Empty;

    [JsonPropertyName("adresse_livraison")]
    public string AdresseLivraison { get; set; } = string.Empty;

    [JsonPropertyName("prix_total")]
    public decimal PrixTotal { get; set; }

    [JsonPropertyName("utilisateur")]
    public UserProfileDto? Utilisateur { get; set; }

    [JsonPropertyName("produits")]
    public List<ProduitDetailDto>? Produits { get; set; }
}
