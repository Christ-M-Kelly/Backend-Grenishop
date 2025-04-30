using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendGrenishop.DbContext;
using BackendGrenishop.Modeles;

namespace BackendGrenishop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProduitController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProduitController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Produit
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetProduits()
    {
        var query = @"
            SELECT 
                p.id_produit,
                p.Nom AS nom_produit,
                p.Etat,
                m.nom_modele,
                m.prix_neuf,
                m.prix_occasion,
                mar.Nom AS nom_marque
            FROM 
                Produits p
            JOIN 
                Modele m ON p.id_modele = m.id_modele
            JOIN 
                Marque mar ON m.id_marque = mar.id_marque";

        var produits = await _context.Database
            .SqlQueryRaw<ProduitDetailDto>(query)
            .ToListAsync();

        return Ok(produits);
    }

    // GET: api/Produit/5
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetProduit(int id)
    {
        var query = @"
            SELECT 
                p.id_produit,
                p.Nom AS nom_produit,
                p.Etat,
                m.nom_modele,
                m.prix_neuf,
                m.prix_occasion,
                mar.Nom AS nom_marque
            FROM 
                Produits p
            JOIN 
                Modele m ON p.id_modele = m.id_modele
            JOIN 
                Marque mar ON m.id_marque = mar.id_marque
            WHERE 
                p.id_produit = {0}";

        var produit = await _context.Database
            .SqlQueryRaw<ProduitDetailDto>(query, id)
            .FirstOrDefaultAsync();

        if (produit == null)
        {
            return NotFound();
        }

        return Ok(produit);
    }

    // POST: api/Produit
    [HttpPost]
    public async Task<ActionResult<Produit>> PostProduit(Produit produit)
    {
        _context.Produits.Add(produit);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduit), new { id = produit.id_produit }, produit);
    }

    // PUT: api/Produit/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduit(int id, Produit produit)
    {
        if (id != produit.id_produit)
        {
            return BadRequest();
        }

        _context.Entry(produit).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProduitExists(id))
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

    // DELETE: api/Produit/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduit(int id)
    {
        var produit = await _context.Produits.FindAsync(id);
        if (produit == null)
        {
            return NotFound();
        }

        _context.Produits.Remove(produit);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProduitExists(int id)
    {
        return _context.Produits.Any(e => e.id_produit == id);
    }
}

public class ProduitDetailDto
{
    public int id_produit { get; set; }
    public string nom_produit { get; set; } = string.Empty;
    public string Etat { get; set; } = string.Empty;
    public string nom_modele { get; set; } = string.Empty;
    public decimal prix_neuf { get; set; }
    public decimal prix_occasion { get; set; }
    public string nom_marque { get; set; } = string.Empty;
} 