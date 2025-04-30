public class CommandeDto
{
    public int IdCompte { get; set; }
    public required List<ProduitCommandeDto> Produits { get; set; }
}

public class ProduitCommandeDto
{
    public int IdProduit { get; set; }
    public int Quantite { get; set; }
    public required string Etat { get; set; }
}
