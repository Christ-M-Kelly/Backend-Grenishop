using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendGrenishop.DbContext;
using BackendGrenishop.Modeles;

namespace BackendGrenishop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommandeController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CommandeController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Commandes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Commande>>> GetCommandes()
    {
        return await _context.Commandes.ToListAsync();
    }

    // GET: api/Commandes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Commande>> GetCommande(int id)
    {
        var commande = await _context.Commandes.FindAsync(id);

        if (commande == null)
        {
            return NotFound();
        }

        return commande;
    }

    // POST: api/Commandes
    [HttpPost]
    public async Task<ActionResult<Commande>> PostCommande(Commande commande)
    {
        _context.Commandes.Add(commande);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCommande), new { id = commande.id_commande }, commande);
    }

    // PUT: api/Commandes/5
    [HttpPut("{id}")]
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
    public async Task<IActionResult> DeleteCommande(int id)
    {
        var commande = await _context.Commandes.FindAsync(id);
        if (commande == null)
        {
            return NotFound();
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