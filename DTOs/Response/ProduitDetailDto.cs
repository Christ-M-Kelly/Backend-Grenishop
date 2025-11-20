using System.Text.Json.Serialization;

namespace BackendGrenishop.DTOs.Response;

public class ProduitDetailDto
{
    [JsonPropertyName("id_produit")]
    public int IdProduit { get; set; }

    [JsonPropertyName("nom_produit")]
    public string NomProduit { get; set; } = string.Empty;

    [JsonPropertyName("etat")]
    public string Etat { get; set; } = string.Empty;

    [JsonPropertyName("nom_modele")]
    public string NomModele { get; set; } = string.Empty;

    [JsonPropertyName("prix_neuf")]
    public decimal PrixNeuf { get; set; }

    [JsonPropertyName("prix_occasion")]
    public decimal PrixOccasion { get; set; }

    [JsonPropertyName("nom_marque")]
    public string NomMarque { get; set; } = string.Empty;

    [JsonPropertyName("prix")]
    public decimal Prix => Etat == "Neuf" ? PrixNeuf : PrixOccasion;
}
