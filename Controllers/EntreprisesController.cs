using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendGrenishop.DbContext;
using BackendGrenishop.Modeles;

namespace BackendGrenishop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EntreprisesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EntreprisesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Entreprises
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Entreprise>>> GetEntreprises()
    {
        return await _context.Entreprises.ToListAsync();
    }

    // GET: api/Entreprises/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Entreprise>> GetEntreprise(int id)
    {
        var entreprise = await _context.Entreprises.FindAsync(id);

        if (entreprise == null)
        {
            return NotFound();
        }

        return entreprise;
    }

    // POST: api/Entreprises
    [HttpPost]
    public async Task<ActionResult<Entreprise>> PostEntreprise(Entreprise entreprise)
    {
        _context.Entreprises.Add(entreprise);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEntreprise), new { id = entreprise.EntrepriseID }, entreprise);
    }

    // PUT: api/Entreprises/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEntreprise(int id, Entreprise entreprise)
    {
        if (id != entreprise.EntrepriseID)
        {
            return BadRequest();
        }

        _context.Entry(entreprise).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Entreprises.Any(e => e.EntrepriseID == id))
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

    // DELETE: api/Entreprises/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEntreprise(int id)
    {
        var entreprise = await _context.Entreprises.FindAsync(id);
        if (entreprise == null)
        {
            return NotFound();
        }

        _context.Entreprises.Remove(entreprise);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
