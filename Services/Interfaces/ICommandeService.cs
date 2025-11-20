using BackendGrenishop.DTOs.Request;
using BackendGrenishop.DTOs.Response;

namespace BackendGrenishop.Services.Interfaces;

public interface ICommandeService
{
    Task<CommandeResponseDto> CreateCommandeAsync(CreateCommandeDto dto, string userId);
    Task<CommandeResponseDto?> GetCommandeByIdAsync(int id);
    Task<IEnumerable<CommandeResponseDto>> GetAllCommandesAsync();
    Task<IEnumerable<CommandeResponseDto>> GetCommandesByUserIdAsync(string userId);
    Task<bool> UpdateCommandeStatusAsync(int id, string status);
    Task<bool> DeleteCommandeAsync(int id);
}
