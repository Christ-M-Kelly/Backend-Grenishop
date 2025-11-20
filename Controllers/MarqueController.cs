using Microsoft.AspNetCore.Mvc;
using BackendGrenishop.Services.Interfaces;

namespace BackendGrenishop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarquesController : ControllerBase
{
    private readonly IMarqueService _marqueService;
    private readonly ILogger<MarquesController> _logger;

    public MarquesController(IMarqueService marqueService, ILogger<MarquesController> logger)
    {
        _marqueService = marqueService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetMarques([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _marqueService.GetMarquesAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMarque(int id)
    {
        var marque = await _marqueService.GetMarqueByIdAsync(id);
        
        if (marque == null)
        {
            return NotFound(new { message = "Marque non trouvée" });
        }

        return Ok(marque);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMarque([FromBody] CreateMarqueDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var marque = await _marqueService.CreateMarqueAsync(dto.Nom);
            return CreatedAtAction(nameof(GetMarque), new { id = marque.id_marque }, marque);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating marque");
            throw;
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMarque(int id, [FromBody] CreateMarqueDto dto)
    {
        var updated = await _marqueService.UpdateMarqueAsync(id, dto.Nom);
        
        if (!updated)
        {
            return NotFound(new { message = "Marque non trouvée" });
        }

        return Ok(new { message = "Marque mise à jour avec succès" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMarque(int id)
    {
        try
        {
            var deleted = await _marqueService.DeleteMarqueAsync(id);
            
            if (!deleted)
            {
                return NotFound(new { message = "Marque non trouvée" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting marque {MarqueId}", id);
            throw;
        }
    }
}

public class CreateMarqueDto
{
    public required string Nom { get; set; }
}
