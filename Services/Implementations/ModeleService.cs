using BackendGrenishop.Modeles;
using BackendGrenishop.DTOs.Response;
using BackendGrenishop.Services.Interfaces;
using BackendGrenishop.Repositories.Interfaces;
using BackendGrenishop.Common.Exceptions;

namespace BackendGrenishop.Services.Implementations;

public class ModeleService : IModeleService
{
    private readonly IModeleRepository _modeleRepository;
    private readonly IMarqueRepository _marqueRepository;
    private readonly ILogger<ModeleService> _logger;

    public ModeleService(
        IModeleRepository modeleRepository,
        IMarqueRepository marqueRepository,
        ILogger<ModeleService> logger)
    {
        _modeleRepository = modeleRepository;
        _marqueRepository = marqueRepository;
        _logger = logger;
    }

    public async Task<Modele?> GetModeleByIdAsync(int id)
    {
        return await _modeleRepository.GetModeleWithDetailsAsync(id);
    }

    public async Task<PagedResultDto<Modele>> GetModelesAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        var (items, totalCount) = await _modeleRepository.GetPagedModelesAsync(page, pageSize);

        return new PagedResultDto<Modele>
        {
            Data = items.ToList(),
            Pagination = new PaginationMetadata
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            }
        };
    }

    public async Task<IEnumerable<Modele>> GetModelesByMarqueAsync(int marqueId)
    {
        return await _modeleRepository.GetModelesByMarqueAsync(marqueId);
    }

    public async Task<Modele> CreateModeleAsync(Modele modele)
    {
        var marque = await _marqueRepository.GetByIdAsync(modele.id_marque);
        if (marque == null)
        {
            throw new NotFoundException("Marque", modele.id_marque);
        }

        await _modeleRepository.AddAsync(modele);
        _logger.LogInformation("Modele {ModeleId} created", modele.id_modele);
        
        return modele;
    }

    public async Task<bool> UpdateModeleAsync(Modele modele)
    {
        var existing = await _modeleRepository.GetByIdAsync(modele.id_modele);
        if (existing == null)
            return false;

        await _modeleRepository.UpdateAsync(modele);
        _logger.LogInformation("Modele {ModeleId} updated", modele.id_modele);
        
        return true;
    }

    public async Task<bool> DeleteModeleAsync(int id)
    {
        var modele = await _modeleRepository.GetModeleWithDetailsAsync(id);
        if (modele == null)
            return false;

        if (modele.Produits != null && modele.Produits.Any())
        {
            throw new BadRequestException("Impossible de supprimer un modèle qui contient des produits");
        }

        await _modeleRepository.DeleteAsync(modele);
        _logger.LogInformation("Modele {ModeleId} deleted", id);
        
        return true;
    }
}
