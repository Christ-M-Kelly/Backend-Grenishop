using BackendGrenishop.DbContext;
using BackendGrenishop.Modeles;
using BackendGrenishop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendGrenishop.Repositories.Implementations;

public class ModeleRepository : GenericRepository<Modele>, IModeleRepository
{
    public ModeleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Modele?> GetModeleWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(m => m.Marque)
            .Include(m => m.Produits)
            .FirstOrDefaultAsync(m => m.id_modele == id);
    }

    public async Task<IEnumerable<Modele>> GetModelesByMarqueAsync(int marqueId)
    {
        return await _dbSet
            .Include(m => m.Marque)
            .Where(m => m.id_marque == marqueId)
            .ToListAsync();
    }

    public async Task<bool> UpdateStockAsync(int modeleId, string etat, int quantity)
    {
        var modele = await _dbSet.FindAsync(modeleId);
        if (modele == null)
            return false;

        if (etat == "Neuf")
        {
            if (modele.nbr_neuf < quantity)
                return false;
            modele.nbr_neuf -= quantity;
        }
        else
        {
            if (modele.nbr_occasion < quantity)
                return false;
            modele.nbr_occasion -= quantity;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(IEnumerable<Modele> Items, int TotalCount)> GetPagedModelesAsync(int page, int pageSize)
    {
        var totalCount = await _dbSet.CountAsync();
        
        var items = await _dbSet
            .Include(m => m.Marque)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
