using BackendGrenishop.Modeles;
using BackendGrenishop.DTOs.Response;
using BackendGrenishop.Services.Interfaces;
using BackendGrenishop.Repositories.Interfaces;
using BackendGrenishop.Common.Exceptions;

namespace BackendGrenishop.Services.Implementations;

public class MarqueService : IMarqueService
{
    private readonly IMarqueRepository _marqueRepository;
    private readonly ILogger<MarqueService> _logger;

    public MarqueService(IMarqueRepository marqueRepository, ILogger<MarqueService> logger)
    {
        _marqueRepository = marqueRepository;
        _logger = logger;
    }

    public async Task<Marque?> GetMarqueByIdAsync(int id)
    {
        return await _marqueRepository.GetByIdAsync(id);
    }

    public async Task<PagedResultDto<Marque>> GetMarquesAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        var (items, totalCount) = await _marqueRepository.GetPagedMarquesAsync(page, pageSize);

        return new PagedResultDto<Marque>
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

    public async Task<Marque> CreateMarqueAsync(string nom)
    {
        var existing = await _marqueRepository.GetByNomAsync(nom);
        if (existing != null)
        {
            throw new BadRequestException($"Une marque avec le nom '{nom}' existe déjà");
        }

        var marque = new Marque { Nom = nom };
        await _marqueRepository.AddAsync(marque);
        
        _logger.LogInformation("Marque {MarqueId} created with name {Nom}", marque.id_marque, nom);
        
        return marque;
    }

    public async Task<bool> UpdateMarqueAsync(int id, string nom)
    {
        var marque = await _marqueRepository.GetByIdAsync(id);
        if (marque == null)
            return false;

        marque.Nom = nom;
        await _marqueRepository.UpdateAsync(marque);
        
        _logger.LogInformation("Marque {MarqueId} updated", id);
        
        return true;
    }

    public async Task<bool> DeleteMarqueAsync(int id)
    {
        var marque = await _marqueRepository.GetMarqueWithModelesAsync(id);
        if (marque == null)
            return false;

        if (marque.Modeles != null && marque.Modeles.Any())
        {
            throw new BadRequestException("Impossible de supprimer une marque qui contient des modèles");
        }

        await _marqueRepository.DeleteAsync(marque);
        _logger.LogInformation("Marque {MarqueId} deleted", id);
        
        return true;
    }
}
