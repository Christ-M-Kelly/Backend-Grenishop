using BackendGrenishop.DbContext;
using BackendGrenishop.Modeles;
using BackendGrenishop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendGrenishop.Repositories.Implementations;

public class MarqueRepository : GenericRepository<Marque>, IMarqueRepository
{
    public MarqueRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Marque?> GetMarqueWithModelesAsync(int id)
    {
        return await _dbSet
            .Include(m => m.Modeles)
            .FirstOrDefaultAsync(m => m.id_marque == id);
    }

    public async Task<Marque?> GetByNomAsync(string nom)
    {
        return await _dbSet
            .FirstOrDefaultAsync(m => m.Nom == nom);
    }

    public async Task<(IEnumerable<Marque> Items, int TotalCount)> GetPagedMarquesAsync(int page, int pageSize)
    {
        var totalCount = await _dbSet.CountAsync();
        
        var items = await _dbSet
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
