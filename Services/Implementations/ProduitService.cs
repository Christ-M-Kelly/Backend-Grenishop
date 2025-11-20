using BackendGrenishop.DTOs.Request;
using BackendGrenishop.DTOs.Response;
using BackendGrenishop.Modeles;
using BackendGrenishop.Services.Interfaces;
using BackendGrenishop.Repositories.Interfaces;
using BackendGrenishop.Common.Exceptions;

namespace BackendGrenishop.Services.Implementations;

public class ProduitService : IProduitService
{
    private readonly IProduitRepository _produitRepository;
    private readonly IModeleRepository _modeleRepository;
    private readonly ILogger<ProduitService> _logger;

    public ProduitService(
        IProduitRepository produitRepository,
        IModeleRepository modeleRepository,
        ILogger<ProduitService> logger)
    {
        _produitRepository = produitRepository;
        _modeleRepository = modeleRepository;
        _logger = logger;
    }

    public async Task<ProduitDetailDto?> GetProduitByIdAsync(int id)
    {
        var produit = await _produitRepository.GetProduitWithDetailsAsync(id);
        if (produit == null)
            return null;

        return MapToProduitDetailDto(produit);
    }

    public async Task<PagedResultDto<ProduitDetailDto>> GetProduitsAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        var (items, totalCount) = await _produitRepository.GetPagedProduitsAsync(page, pageSize);

        return new PagedResultDto<ProduitDetailDto>
        {
            Data = items.Select(MapToProduitDetailDto).ToList(),
            Pagination = new PaginationMetadata
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            }
        };
    }

    public async Task<IEnumerable<ProduitDetailDto>> GetProduitsAvailableAsync()
    {
        var produits = await _produitRepository.GetProduitsAvailableAsync();
        return produits.Select(MapToProduitDetailDto);
    }

    public async Task<ProduitDetailDto> CreateProduitAsync(CreateProduitDto dto)
    {
        var modele = await _modeleRepository.GetModeleWithDetailsAsync(dto.IdModele);
        if (modele == null)
        {
            throw new NotFoundException("Modèle", dto.IdModele);
        }

        var produit = new Produit
        {
            Nom = dto.Nom,
            Etat = dto.Etat,
            id_modele = dto.IdModele
        };

        await _produitRepository.AddAsync(produit);
        _logger.LogInformation("Produit {ProduitId} created", produit.id_produit);

        // Reload with details
        var createdProduit = await _produitRepository.GetProduitWithDetailsAsync(produit.id_produit);
        return MapToProduitDetailDto(createdProduit!);
    }

    public async Task<bool> DeleteProduitAsync(int id)
    {
        var produit = await _produitRepository.GetByIdAsync(id);
        if (produit == null)
            return false;

        if (produit.id_commande != null)
        {
            throw new BadRequestException("Impossible de supprimer un produit qui fait partie d'une commande");
        }

        await _produitRepository.DeleteAsync(produit);
        _logger.LogInformation("Produit {ProduitId} deleted", id);
        
        return true;
    }

    private ProduitDetailDto MapToProduitDetailDto(Produit produit)
    {
        return new ProduitDetailDto
        {
            IdProduit = produit.id_produit,
            NomProduit = produit.Nom,
            Etat = produit.Etat,
            NomModele = produit.Modele?.nom_modele ?? string.Empty,
            PrixNeuf = produit.Modele?.prix_neuf ?? 0,
            PrixOccasion = produit.Modele?.prix_occasion ?? 0,
            NomMarque = produit.Modele?.Marque?.Nom ?? string.Empty
        };
    }
}
