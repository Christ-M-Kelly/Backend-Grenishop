using Microsoft.AspNetCore.Mvc;
using BackendGrenishop.Modeles;
using BackendGrenishop.Services.Interfaces;

namespace BackendGrenishop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModelesController : ControllerBase
{
    private readonly IModeleService _modeleService;
    private readonly ILogger<ModelesController> _logger;

    public ModelesController(IModeleService modeleService, ILogger<ModelesController> logger)
    {
        _modeleService = modeleService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetModeles([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _modeleService.GetModelesAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("by-marque/{marqueId}")]
    public async Task<IActionResult> GetModelesByMarque(int marqueId)
    {
        var modeles = await _modeleService.GetModelesByMarqueAsync(marqueId);
        return Ok(modeles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetModele(int id)
    {
        var modele = await _modeleService.GetModeleByIdAsync(id);
        
        if (modele == null)
        {
            return NotFound(new { message = "Modèle non trouvé" });
        }

        return Ok(modele);
    }

    [HttpPost]
    public async Task<IActionResult> CreateModele([FromBody] Modele modele)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var created = await _modeleService.CreateModeleAsync(modele);
            return CreatedAtAction(nameof(GetModele), new { id = created.id_modele }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating modele");
            throw;
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateModele(int id, [FromBody] Modele modele)
    {
        if (id != modele.id_modele)
        {
            return BadRequest(new { message = "L'ID ne correspond pas" });
        }

        var updated = await _modeleService.UpdateModeleAsync(modele);
        
        if (!updated)
        {
            return NotFound(new { message = "Modèle non trouvé" });
        }

        return Ok(new { message = "Modèle mis à jour avec succès" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteModele(int id)
    {
        try
        {
            var deleted = await _modeleService.DeleteModeleAsync(id);
            
            if (!deleted)
            {
                return NotFound(new { message = "Modèle non trouvé" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting modele {ModeleId}", id);
            throw;
        }
    }
}