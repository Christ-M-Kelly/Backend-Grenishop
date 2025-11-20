using BackendGrenishop.DbContext;
using BackendGrenishop.Modeles;
using BackendGrenishop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendGrenishop.Repositories.Implementations;

public class ProduitRepository : GenericRepository<Produit>, IProduitRepository
{
    public ProduitRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Produit>> GetProduitsWithDetailsAsync()
    {
        return await _dbSet
            .Include(p => p.Modele)
                .ThenInclude(m => m!.Marque)
            .ToListAsync();
    }

    public async Task<Produit?> GetProduitWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(p => p.Modele)
                .ThenInclude(m => m!.Marque)
            .FirstOrDefaultAsync(p => p.id_produit == id);
    }

    public async Task<IEnumerable<Produit>> GetProduitsByModeleAsync(int modeleId)
    {
        return await _dbSet
            .Include(p => p.Modele)
            .Where(p => p.id_modele == modeleId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Produit>> GetProduitsAvailableAsync()
    {
        return await _dbSet
            .Include(p => p.Modele)
                .ThenInclude(m => m!.Marque)
            .Where(p => p.id_commande == null)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Produit> Items, int TotalCount)> GetPagedProduitsAsync(int page, int pageSize)
    {
        var totalCount = await _dbSet.CountAsync();
        
        var items = await _dbSet
            .Include(p => p.Modele)
                .ThenInclude(m => m!.Marque)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
