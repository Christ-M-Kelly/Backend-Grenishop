using BackendGrenishop.DTOs.Request;
using BackendGrenishop.DTOs.Response;
using BackendGrenishop.Modeles;
using BackendGrenishop.Services.Interfaces;
using BackendGrenishop.Repositories.Interfaces;
using BackendGrenishop.Common.Exceptions;
using BackendGrenishop.DbContext;
using Microsoft.AspNetCore.Identity;
using BackendGrenishop.Models;

namespace BackendGrenishop.Services.Implementations;

public class CommandeService : ICommandeService
{
    private readonly ICommandeRepository _commandeRepository;
    private readonly IProduitRepository _produitRepository;
    private readonly IModeleRepository _modeleRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CommandeService> _logger;

    public CommandeService(
        ICommandeRepository commandeRepository,
        IProduitRepository produitRepository,
        IModeleRepository modeleRepository,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        ILogger<CommandeService> logger)
    {
        _commandeRepository = commandeRepository;
        _produitRepository = produitRepository;
        _modeleRepository = modeleRepository;
        _userManager = userManager;
        _context = context;
        _logger = logger;
    }

    public async Task<CommandeResponseDto> CreateCommandeAsync(CreateCommandeDto dto, string userId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            // Verify user exists
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("Utilisateur", userId);
            }

            // Create commande
            var commande = new Commande
            {
                UserId = userId,
                date_commande = DateTime.UtcNow,
                status_commande = "En attente",
                adresse_livraison = dto.AdresseLivraison,
                prix_total = 0
            };

            await _commandeRepository.AddAsync(commande);

            // Process products and calculate total
            decimal prixTotal = 0;
            var produitsDetails = new List<ProduitDetailDto>();

            foreach (var produitDto in dto.Produits)
            {
                var produit = await _produitRepository.GetProduitWithDetailsAsync(produitDto.IdProduit);
                
                if (produit == null)
                {
                    throw new NotFoundException("Produit", produitDto.IdProduit);
                }

                if (produit.id_commande != null)
                {
                    throw new BadRequestException($"Le produit {produit.id_produit} est déjà dans une commande");
                }

                if (produit.Modele == null)
                {
                    throw new BadRequestException($"Le modèle du produit {produit.id_produit} n'existe pas");
                }

                // Calculate price
                decimal prixProduit = produit.Etat == "Neuf" 
                    ? produit.Modele.prix_neuf 
                    : produit.Modele.prix_occasion;
                
                prixTotal += prixProduit * produitDto.Quantite;

                // Update stock
                var stockUpdated = await _modeleRepository.UpdateStockAsync(
                    produit.id_modele, 
                    produit.Etat, 
                    produitDto.Quantite);

                if (!stockUpdated)
                {
                    throw new BadRequestException($"Stock insuffisant pour {produit.Nom} ({produit.Etat})");
                }

                // Assign product to commande
                produit.id_commande = commande.id_commande;
                await _produitRepository.UpdateAsync(produit);

                // Add to details
                produitsDetails.Add(new ProduitDetailDto
                {
                    IdProduit = produit.id_produit,
                    NomProduit = produit.Nom,
                    Etat = produit.Etat,
                    NomModele = produit.Modele.nom_modele,
                    PrixNeuf = produit.Modele.prix_neuf,
                    PrixOccasion = produit.Modele.prix_occasion,
                    NomMarque = produit.Modele.Marque?.Nom ?? string.Empty
                });
            }

            // Update total price
            commande.prix_total = prixTotal;
            await _commandeRepository.UpdateAsync(commande);

            await transaction.CommitAsync();

            _logger.LogInformation("Commande {CommandeId} created successfully for user {UserId}", commande.id_commande, userId);

            return new CommandeResponseDto
            {
                IdCommande = commande.id_commande,
                DateCommande = commande.date_commande,
                StatusCommande = commande.status_commande,
                AdresseLivraison = commande.adresse_livraison,
                PrixTotal = commande.prix_total,
                Utilisateur = new UserProfileDto
                {
                    Id = user.Id,
                    Nom = user.Nom,
                    Prenom = user.Prenom,
                    Email = user.Email ?? string.Empty,
                    DateInscription = user.DateInscription
                },
                Produits = produitsDetails
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error creating commande for user {UserId}", userId);
            throw;
        }
    }

    public async Task<CommandeResponseDto?> GetCommandeByIdAsync(int id)
    {
        var commande = await _commandeRepository.GetCommandeWithDetailsAsync(id);
        if (commande == null)
            return null;

        return MapToCommandeResponseDto(commande);
    }

    public async Task<IEnumerable<CommandeResponseDto>> GetAllCommandesAsync()
    {
        var commandes = await _commandeRepository.GetCommandesWithDetailsAsync();
        return commandes.Select(MapToCommandeResponseDto);
    }

    public async Task<IEnumerable<CommandeResponseDto>> GetCommandesByUserIdAsync(string userId)
    {
        var commandes = await _commandeRepository.GetCommandesByUserIdAsync(userId);
        return commandes.Select(MapToCommandeResponseDto);
    }

    public async Task<bool> UpdateCommandeStatusAsync(int id, string status)
    {
        var commande = await _commandeRepository.GetByIdAsync(id);
        if (commande == null)
            return false;

        commande.status_commande = status;
        
        if (status == "Livrée")
        {
            commande.date_reception = DateTime.UtcNow;
        }

        await _commandeRepository.UpdateAsync(commande);
        _logger.LogInformation("Commande {CommandeId} status updated to {Status}", id, status);
        
        return true;
    }

    public async Task<bool> DeleteCommandeAsync(int id)
    {
        var commande = await _commandeRepository.GetCommandeWithDetailsAsync(id);
        if (commande == null)
            return false;

        // Free products from commande
        if (commande.Produits != null)
        {
            foreach (var produit in commande.Produits)
            {
                produit.id_commande = null;
                await _produitRepository.UpdateAsync(produit);
            }
        }

        await _commandeRepository.DeleteAsync(commande);
        _logger.LogInformation("Commande {CommandeId} deleted", id);
        
        return true;
    }

    private CommandeResponseDto MapToCommandeResponseDto(Commande commande)
    {
        return new CommandeResponseDto
        {
            IdCommande = commande.id_commande,
            DateCommande = commande.date_commande,
            DateReception = commande.date_reception,
            StatusCommande = commande.status_commande,
            AdresseLivraison = commande.adresse_livraison,
            PrixTotal = commande.prix_total,
            Utilisateur = commande.ApplicationUser != null ? new UserProfileDto
            {
                Id = commande.ApplicationUser.Id,
                Nom = commande.ApplicationUser.Nom,
                Prenom = commande.ApplicationUser.Prenom,
                Email = commande.ApplicationUser.Email ?? string.Empty,
                DateInscription = commande.ApplicationUser.DateInscription
            } : null,
            Produits = commande.Produits?.Select(p => new ProduitDetailDto
            {
                IdProduit = p.id_produit,
                NomProduit = p.Nom,
                Etat = p.Etat,
                NomModele = p.Modele?.nom_modele ?? string.Empty,
                PrixNeuf = p.Modele?.prix_neuf ?? 0,
                PrixOccasion = p.Modele?.prix_occasion ?? 0,
                NomMarque = p.Modele?.Marque?.Nom ?? string.Empty
            }).ToList()
        };
    }
}
