using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendGrenishop.Modeles;
using BackendGrenishop.DbContext;
using System.Text.Json.Serialization;

namespace BackendGrenishop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComptesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ComptesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Méthode pour hacher le mot de passe avec bcrypt
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Méthode pour vérifier le mot de passe
        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        // Inscription
        [HttpPost("inscription")]
        public async Task<IActionResult> Inscription([FromBody] InscriptionModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Vérifier si l'email existe déjà
                if (await _context.Comptes.AnyAsync(c => c.Email == model.Email))
                {
                    return BadRequest(new { message = "Cet email est déjà utilisé" });
                }

                var compte = new Compte
                {
                    Nom = model.Nom,
                    Prenom = model.Prenom,
                    Email = model.Email,
                    MotDePasseHash = HashPassword(model.MotDePasse),
                    Date_inscription = DateTime.UtcNow
                };

                _context.Comptes.Add(compte);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Inscription réussie" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur serveur", details = ex.Message });
            }
        }

        // Connexion
        [HttpPost("connexion")]
        public async Task<IActionResult> Connexion([FromBody] ConnexionModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var compte = await _context.Comptes.FirstOrDefaultAsync(c => c.Email == model.Email);

            if (compte == null || !VerifyPassword(model.MotDePasse, compte.MotDePasseHash))
                return Unauthorized(new { message = "Email ou mot de passe incorrect" });

            return Ok(new
            {
                message = "Connexion réussie",
                compte = new
                {
                    id = compte.id_compte,
                    nom = compte.Nom,
                    prenom = compte.Prenom,
                    email = compte.Email
                }
            });
        }
    }

    // Modèle pour Inscription
    public class InscriptionModel
    {
        [JsonPropertyName("nom")]
        public required string Nom { get; set; }

        [JsonPropertyName("prenom")]
        public required string Prenom { get; set; }

        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("motDePasse")]
        public required string MotDePasse { get; set; }
    }

    // Modèle pour Connexion
    public class ConnexionModel
    {
        public required string Email { get; set; }
        public required string MotDePasse { get; set; }
    }
}
