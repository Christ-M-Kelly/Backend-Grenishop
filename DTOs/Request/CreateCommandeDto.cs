using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackendGrenishop.DTOs.Request;

public class CreateCommandeDto
{
    [Required(ErrorMessage = "L'adresse de livraison est obligatoire")]
    [StringLength(200, ErrorMessage = "L'adresse ne peut pas dépasser 200 caractères")]
    [JsonPropertyName("adresse_livraison")]
    public required string AdresseLivraison { get; set; }

    [Required(ErrorMessage = "La liste des produits est obligatoire")]
    [MinLength(1, ErrorMessage = "La commande doit contenir au moins un produit")]
    [JsonPropertyName("produits")]
    public required List<ProduitCommandeDto> Produits { get; set; }
}

public class ProduitCommandeDto
{
    [Required(ErrorMessage = "L'ID du produit est obligatoire")]
    [Range(1, int.MaxValue, ErrorMessage = "L'ID du produit doit être supérieur à 0")]
    [JsonPropertyName("id_produit")]
    public int IdProduit { get; set; }

    [Required(ErrorMessage = "La quantité est obligatoire")]
    [Range(1, 100, ErrorMessage = "La quantité doit être entre 1 et 100")]
    [JsonPropertyName("quantite")]
    public int Quantite { get; set; }
}
