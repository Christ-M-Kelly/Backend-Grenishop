using BackendGrenishop.DbContext;
using BackendGrenishop.Modeles;
using BackendGrenishop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendGrenishop.Repositories.Implementations;

public class CommandeRepository : GenericRepository<Commande>, ICommandeRepository
{
    public CommandeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Commande>> GetCommandesWithDetailsAsync()
    {
        return await _dbSet
            .Include(c => c.ApplicationUser)
            .Include(c => c.Produits)
                .ThenInclude(p => p.Modele)
                    .ThenInclude(m => m!.Marque)
            .ToListAsync();
    }

    public async Task<Commande?> GetCommandeWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(c => c.ApplicationUser)
            .Include(c => c.Produits)
                .ThenInclude(p => p.Modele)
                    .ThenInclude(m => m!.Marque)
            .FirstOrDefaultAsync(c => c.id_commande == id);
    }

    public async Task<IEnumerable<Commande>> GetCommandesByUserIdAsync(string userId)
    {
        return await _dbSet
            .Include(c => c.Produits)
                .ThenInclude(p => p.Modele)
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.date_commande)
            .ToListAsync();
    }

    public async Task<IEnumerable<Commande>> GetCommandesByStatusAsync(string status)
    {
        return await _dbSet
            .Include(c => c.ApplicationUser)
            .Include(c => c.Produits)
            .Where(c => c.status_commande == status)
            .ToListAsync();
    }
}
