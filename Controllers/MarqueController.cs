using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendGrenishop.DbContext;
using BackendGrenishop.Modeles;

namespace BackendGrenishop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarqueController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MarqueController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Marque
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Marque>>> GetMarques()
    {
        return await _context.Marque.ToListAsync();
    }

    // GET: api/Marque/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Marque>> GetMarque(int id)
    {
        var marque = await _context.Marque.FindAsync(id);

        if (marque == null)
        {
            return NotFound();
        }

        return marque;
    }

    // POST: api/Marque
    [HttpPost]
    public async Task<ActionResult<Marque>> PostMarque(Marque marque)
    {
        _context.Marque.Add(marque);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMarque), new { id = marque.id_marque }, marque);
    }

    // PUT: api/Marque/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMarque(int id, Marque marque)
    {
        if (id != marque.id_marque)
        {
            return BadRequest();
        }

        _context.Entry(marque).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MarqueExists(id))
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

    // DELETE: api/Marque/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMarque(int id)
    {
        var marque = await _context.Marque.FindAsync(id);
        if (marque == null)
        {
            return NotFound();
        }

        _context.Marque.Remove(marque);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MarqueExists(int id)
    {
        return _context.Marque.Any(e => e.id_marque == id);
    }
}
