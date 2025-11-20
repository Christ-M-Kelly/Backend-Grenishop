using BackendGrenishop.DTOs.Request;
using BackendGrenishop.DTOs.Response;

namespace BackendGrenishop.Services.Interfaces;

public interface IProduitService
{
    Task<ProduitDetailDto?> GetProduitByIdAsync(int id);
    Task<PagedResultDto<ProduitDetailDto>> GetProduitsAsync(int page, int pageSize);
    Task<IEnumerable<ProduitDetailDto>> GetProduitsAvailableAsync();
    Task<ProduitDetailDto> CreateProduitAsync(CreateProduitDto dto);
    Task<bool> DeleteProduitAsync(int id);
}
