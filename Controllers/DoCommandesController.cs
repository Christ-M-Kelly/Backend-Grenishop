using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BackendGrenishop.DbContext;
using BackendGrenishop.Modeles;
using System.Text.Json.Serialization;

namespace BackendGrenishop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommandesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CommandesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Commandes
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Commande>>> GetCommandes()
    {
        try
        {
            var commandes = await _context.Commandes
                .Include(c => c.Compte)
                .Include(c => c.Produits)
                .ThenInclude(p => p.Modele)
                .ThenInclude(m => m.Marque)
                .ToListAsync();

            return Ok(commandes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur interne du serveur: {ex.Message}");
        }
    }

    // GET: api/Commandes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Commande>> GetCommande(int id)
    {
        var commande = await _context.Commandes
            .Include(c => c.Compte)
            .Include(c => c.Produits)
            .FirstOrDefaultAsync(c => c.id_commande == id);

        if (commande == null)
        {
            return NotFound();
        }

        return commande;
    }

    // POST: api/Commandes
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Commande>> PostCommande([FromBody] CommandeDto dto)
    {
        // Vérifier si le compte existe
        var compte = await _context.Comptes.FindAsync(dto.IdCompte);
        if (compte == null)
        {
            return BadRequest("Compte non trouvé");
        }

        // Créer la commande
        var commande = new Commande
        {
            id_compte = dto.IdCompte,
            date_commande = DateTime.UtcNow,
            status_commande = "En attente",
            adresse_livraison = dto.AdresseLivraison,
            prix_total = 0 // Initialiser à 0
        };

        _context.Commandes.Add(commande);
        await _context.SaveChangesAsync();

        // Ajouter les produits à la commande et calculer le prix total
        decimal prixTotal = 0;
        foreach (var produitDto in dto.Produits)
        {
            var produit = await _context.Produits
                .Include(p => p.Modele)
                .FirstOrDefaultAsync(p => p.id_produit == produitDto.IdProduit);
                
            if (produit == null)
            {
                return BadRequest($"Produit avec l'ID {produitDto.IdProduit} non trouvé");
            }

            // Vérifier si le produit est déjà dans une commande
            if (produit.id_commande != null)
            {
                return BadRequest($"Le produit {produit.id_produit} est déjà dans une commande");
            }

            // Calculer le prix du produit
            decimal prixProduit = produit.Etat == "Neuf" 
                ? produit.Modele.prix_neuf 
                : produit.Modele.prix_occasion;
            
            prixTotal += prixProduit * produitDto.Quantite;

            // Mettre à jour la commande du produit
            produit.id_commande = commande.id_commande;
        }

        // Mettre à jour le prix total de la commande
        commande.prix_total = prixTotal;
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCommande), new { id = commande.id_commande }, commande);
    }

    // PUT: api/Commandes/5
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutCommande(int id, Commande commande)
    {
        if (id != commande.id_commande)
        {
            return BadRequest();
        }

        _context.Entry(commande).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CommandeExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Commandes/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteCommande(int id)
    {
        var commande = await _context.Commandes
            .Include(c => c.Produits)
            .FirstOrDefaultAsync(c => c.id_commande == id);

        if (commande == null)
        {
            return NotFound();
        }

        // Libérer les produits de la commande
        foreach (var produit in commande.Produits)
        {
            produit.id_commande = null;
        }

        _context.Commandes.Remove(commande);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CommandeExists(int id)
    {
        return _context.Commandes.Any(e => e.id_commande == id);
    }
}

// DTO pour la création de commande
public class CommandeDto
{
    [JsonPropertyName("id_compte")]
    public int IdCompte { get; set; }

    [JsonPropertyName("adresse_livraison")]
    public required string AdresseLivraison { get; set; }

    [JsonPropertyName("produits")]
    public required List<ProduitCommandeDto> Produits { get; set; }
}

public class ProduitCommandeDto
{
    [JsonPropertyName("id_produit")]
    public int IdProduit { get; set; }

    [JsonPropertyName("quantite")]
    public int Quantite { get; set; }
}
