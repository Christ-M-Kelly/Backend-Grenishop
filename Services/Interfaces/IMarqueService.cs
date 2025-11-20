using BackendGrenishop.Modeles;
using BackendGrenishop.DTOs.Response;

namespace BackendGrenishop.Services.Interfaces;

public interface IMarqueService
{
    Task<Marque?> GetMarqueByIdAsync(int id);
    Task<PagedResultDto<Marque>> GetMarquesAsync(int page, int pageSize);
    Task<Marque> CreateMarqueAsync(string nom);
    Task<bool> UpdateMarqueAsync(int id, string nom);
    Task<bool> DeleteMarqueAsync(int id);
}
