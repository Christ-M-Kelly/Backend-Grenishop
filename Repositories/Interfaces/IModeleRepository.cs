using BackendGrenishop.Modeles;

namespace BackendGrenishop.Repositories.Interfaces;

public interface IModeleRepository : IGenericRepository<Modele>
{
    Task<Modele?> GetModeleWithDetailsAsync(int id);
    Task<IEnumerable<Modele>> GetModelesByMarqueAsync(int marqueId);
    Task<bool> UpdateStockAsync(int modeleId, string etat, int quantity);
    Task<(IEnumerable<Modele> Items, int TotalCount)> GetPagedModelesAsync(int page, int pageSize);
}
