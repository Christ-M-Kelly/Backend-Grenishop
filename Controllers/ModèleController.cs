using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendGrenishop.DbContext;
using BackendGrenishop.Modeles;

namespace BackendGrenishop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModeleController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ModeleController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Modele
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Modele>>> GetModeles()
    {
        return await _context.Modele
            .Include(m => m.Marque)
            .ToListAsync();
    }

    // GET: api/Modele/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Modele>> GetModele(int id)
    {
        var modele = await _context.Modele
            .Include(m => m.Marque)
            .FirstOrDefaultAsync(m => m.id_modele == id);

        if (modele == null)
        {
            return NotFound();
        }

        return modele;
    }

    // POST: api/Modele
    [HttpPost]
    public async Task<ActionResult<Modele>> PostModele(Modele modele)
    {
        _context.Modele.Add(modele);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetModele), new { id = modele.id_modele }, modele);
    }

    // PUT: api/Modele/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutModele(int id, Modele modele)
    {
        if (id != modele.id_modele)
        {
            return BadRequest();
        }

        _context.Entry(modele).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ModeleExists(id))
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

    // DELETE: api/Modele/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteModele(int id)
    {
        var modele = await _context.Modele.FindAsync(id);
        if (modele == null)
        {
            return NotFound();
        }

        _context.Modele.Remove(modele);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ModeleExists(int id)
    {
        return _context.Modele.Any(e => e.id_modele == id);
    }
} 