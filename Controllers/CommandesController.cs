using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BackendGrenishop.DTOs.Request;
using BackendGrenishop.Services.Interfaces;

namespace BackendGrenishop.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommandesController : ControllerBase
{
    private readonly ICommandeService _commandeService;
    private readonly ILogger<CommandesController> _logger;

    public CommandesController(ICommandeService commandeService, ILogger<CommandesController> logger)
    {
        _commandeService = commandeService;
        _logger = logger;
    }

    /// <summary>
    /// Get all commandes (admin)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCommandes()
    {
        var commandes = await _commandeService.GetAllCommandesAsync();
        return Ok(commandes);
    }

    /// <summary>
    /// Get commande by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCommande(int id)
    {
        var commande = await _commandeService.GetCommandeByIdAsync(id);
        
        if (commande == null)
        {
            return NotFound(new { message = "Commande non trouvée" });
        }

        return Ok(commande);
    }

    /// <summary>
    /// Get current user's commandes
    /// </summary>
    [HttpGet("my-orders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyCommandes()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { message = "Token invalide" });
        }

        var commandes = await _commandeService.GetCommandesByUserIdAsync(userId);
        return Ok(commandes);
    }

    /// <summary>
    /// Create a new commande
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCommande([FromBody] CreateCommandeDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { message = "Token invalide" });
        }

        try
        {
            var commande = await _commandeService.CreateCommandeAsync(dto, userId);
            return CreatedAtAction(nameof(GetCommande), new { id = commande.IdCommande }, commande);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating commande for user {UserId}", userId);
            throw; // Will be caught by ExceptionHandlingMiddleware
        }
    }

    /// <summary>
    /// Update commande status
    /// </summary>
    [HttpPatch("{id}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusDto dto)
    {
        var updated = await _commandeService.UpdateCommandeStatusAsync(id, dto.Status);
        
        if (!updated)
        {
            return NotFound(new { message = "Commande non trouvée" });
        }

        return Ok(new { message = "Statut mis à jour avec succès" });
    }

    /// <summary>
    /// Delete a commande
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCommande(int id)
    {
        var deleted = await _commandeService.DeleteCommandeAsync(id);
        
        if (!deleted)
        {
            return NotFound(new { message = "Commande non trouvée" });
        }

        return NoContent();
    }
}

public class UpdateStatusDto
{
    public required string Status { get; set; }
}
