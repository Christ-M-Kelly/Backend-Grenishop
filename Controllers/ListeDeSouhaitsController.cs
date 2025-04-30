using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendGrenishop.DbContext;
using BackendGrenishop.Modeles;

namespace BackendGrenishop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListeDeSouhaitsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ListeDeSouhaitsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/ListeDeSouhaits
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ListeDeSouhaits>>> GetListeDeSouhaits()
    {
        return await _context.ListeDeSouhaits.ToListAsync();
    }

    // GET: api/ListeDeSouhaits/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ListeDeSouhaits>> GetListeDeSouhaits(int id)
    {
        var listeDeSouhaits = await _context.ListeDeSouhaits.FindAsync(id);

        if (listeDeSouhaits == null)
        {
            return NotFound();
        }

        return listeDeSouhaits;
    }

    // POST: api/ListeDeSouhaits
    [HttpPost]
    public async Task<ActionResult<ListeDeSouhaits>> PostListeDeSouhaits(ListeDeSouhaits listeDeSouhaits)
    {
        _context.ListeDeSouhaits.Add(listeDeSouhaits);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetListeDeSouhaits), new { id = listeDeSouhaits.id_liste }, listeDeSouhaits);
    }

    // DELETE: api/ListeDeSouhaits/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteListeDeSouhaits(int id)
    {
        var listeDeSouhaits = await _context.ListeDeSouhaits.FindAsync(id);
        if (listeDeSouhaits == null)
        {
            return NotFound();
        }

        _context.ListeDeSouhaits.Remove(listeDeSouhaits);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ListeDeSouhaitsExists(int id)
    {
        return _context.ListeDeSouhaits.Any(e => e.id_liste == id);
    }
} 