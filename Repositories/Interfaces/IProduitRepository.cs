using BackendGrenishop.Modeles;

namespace BackendGrenishop.Repositories.Interfaces;

public interface IProduitRepository : IGenericRepository<Produit>
{
    Task<IEnumerable<Produit>> GetProduitsWithDetailsAsync();
    Task<Produit?> GetProduitWithDetailsAsync(int id);
    Task<IEnumerable<Produit>> GetProduitsByModeleAsync(int modeleId);
    Task<IEnumerable<Produit>> GetProduitsAvailableAsync();
    Task<(IEnumerable<Produit> Items, int TotalCount)> GetPagedProduitsAsync(int page, int pageSize);
}
