using BackendGrenishop.Modeles;

namespace BackendGrenishop.Repositories.Interfaces;

public interface IMarqueRepository : IGenericRepository<Marque>
{
    Task<Marque?> GetMarqueWithModelesAsync(int id);
    Task<Marque?> GetByNomAsync(string nom);
    Task<(IEnumerable<Marque> Items, int TotalCount)> GetPagedMarquesAsync(int page, int pageSize);
}
