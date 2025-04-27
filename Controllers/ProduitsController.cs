using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendGrenishop.DbContext;
using BackendGrenishop.Modeles;

namespace BackendGrenishop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProduitsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProduitsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Produits
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produit>>> GetProduits()
    {
        return await _context.Produits.Include(p => p.Entreprise).ToListAsync();
    }

    // GET: api/Produits/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Produit>> GetProduit(int id)
    {
        var produit = await _context.Produits.Include(p => p.Entreprise).FirstOrDefaultAsync(p => p.ProduitID == id);
        if (produit == null)
        {
            return NotFound();
        }
        return produit;
    }

    // POST: api/Produits
    [HttpPost]
    public async Task<ActionResult<Produit>> PostProduit(Produit produit)
    {
        _context.Produits.Add(produit);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProduit), new { id = produit.ProduitID }, produit);
    }

    // PUT: api/Produits/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduit(int id, Produit produit)
    {
        if (id != produit.ProduitID)
        {
            return BadRequest();
        }
        _context.Entry(produit).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Produits/5
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
}
