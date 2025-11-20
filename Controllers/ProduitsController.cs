using Microsoft.AspNetCore.Mvc;
using BackendGrenishop.DTOs.Request;
using BackendGrenishop.Services.Interfaces;

namespace BackendGrenishop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProduitsController : ControllerBase
{
    private readonly IProduitService _produitService;
    private readonly ILogger<ProduitsController> _logger;

    public ProduitsController(IProduitService produitService, ILogger<ProduitsController> logger)
    {
        _produitService = produitService;
        _logger = logger;
    }

    /// <summary>
    /// Get all produits with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProduits([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _produitService.GetProduitsAsync(page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Get available produits (not in any commande)
    /// </summary>
    [HttpGet("available")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProduitsAvailable()
    {
        var produits = await _produitService.GetProduitsAvailableAsync();
        return Ok(produits);
    }

    /// <summary>
    /// Get produit by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduit(int id)
    {
        var produit = await _produitService.GetProduitByIdAsync(id);
        
        if (produit == null)
        {
            return NotFound(new { message = "Produit non trouvé" });
        }

        return Ok(produit);
    }

    /// <summary>
    /// Create a new produit
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduit([FromBody] CreateProduitDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var produit = await _produitService.CreateProduitAsync(dto);
            return CreatedAtAction(nameof(GetProduit), new { id = produit.IdProduit }, produit);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating produit");
            throw; // Will be caught by ExceptionHandlingMiddleware
        }
    }

    /// <summary>
    /// Delete a produit
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProduit(int id)
    {
        try
        {
            var deleted = await _produitService.DeleteProduitAsync(id);
            
            if (!deleted)
            {
                return NotFound(new { message = "Produit non trouvé" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting produit {ProduitId}", id);
            throw; // Will be caught by ExceptionHandlingMiddleware
        }
    }
}