using BackendGrenishop.Modeles;

namespace BackendGrenishop.Repositories.Interfaces;

public interface ICommandeRepository : IGenericRepository<Commande>
{
    Task<IEnumerable<Commande>> GetCommandesWithDetailsAsync();
    Task<Commande?> GetCommandeWithDetailsAsync(int id);
    Task<IEnumerable<Commande>> GetCommandesByUserIdAsync(string userId);
    Task<IEnumerable<Commande>> GetCommandesByStatusAsync(string status);
}
