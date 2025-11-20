using BackendGrenishop.Modeles;
using BackendGrenishop.DTOs.Response;

namespace BackendGrenishop.Services.Interfaces;

public interface IModeleService
{
    Task<Modele?> GetModeleByIdAsync(int id);
    Task<PagedResultDto<Modele>> GetModelesAsync(int page, int pageSize);
    Task<IEnumerable<Modele>> GetModelesByMarqueAsync(int marqueId);
    Task<Modele> CreateModeleAsync(Modele modele);
    Task<bool> UpdateModeleAsync(Modele modele);
    Task<bool> DeleteModeleAsync(int id);
}
