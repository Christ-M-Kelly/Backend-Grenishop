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
    public async Task<ActionResult<IEnumerable<Produit>>> GetProduits()
    {
        return await _context.Produits
            .Include(p => p.Modele)
            .Include(p => p.Commande)
            .ToListAsync();
    }

    // GET: api/Produit/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Produit>> GetProduit(int id)
    {
        var produit = await _context.Produits
            .Include(p => p.Modele)
            .Include(p => p.Commande)
            .FirstOrDefaultAsync(p => p.id_produit == id);

        if (produit == null)
        {
            return NotFound();
        }

        return produit;
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