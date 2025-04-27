namespace BackendGrenishop.Modeles;

public class Produit
{
    public int ProduitID { get; set; }
    public required string Nom { get; set; }
    public required string Etat { get; set; }
    public int EntrepriseID { get; set; }
    public int Nombre_Neuf { get; set; }
    public int Nombre_Occasion { get; set; }
    public decimal Prix_Neuf { get; set; }
    public decimal Prix_Occasion { get; set; }

    public Entreprise? Entreprise { get; set; }
}
