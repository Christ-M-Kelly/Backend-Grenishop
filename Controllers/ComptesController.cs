using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendGrenishop.Modeles;
using BackendGrenishop.DbContext;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace BackendGrenishop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComptesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public ComptesController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    var errorMessages = new List<string>();
                    
                    foreach (var error in errors)
                    {
                        if (error.Contains("MotDePasse"))
                            errorMessages.Add("Le mot de passe doit contenir au moins 6 caractères");
                        else if (error.Contains("Email"))
                            errorMessages.Add("Veuillez entrer une adresse email valide");
                        else if (error.Contains("Required"))
                            errorMessages.Add("Tous les champs sont obligatoires");
                        else
                            errorMessages.Add(error);
                    }

                    return BadRequest(new { 
                        message = "Erreur de validation",
                        errors = errorMessages
                    });
                }

                // Vérifier si l'email existe déjà
                if (await _context.Comptes.AnyAsync(c => c.Email == model.Email))
                {
                    return BadRequest(new { 
                        message = "Erreur d'inscription",
                        errors = new[] { "Cette adresse email est déjà utilisée" }
                    });
                }

                var compte = new Compte
                {
                    Nom = model.Nom,
                    Prenom = model.Prenom,
                    Email = model.Email,
                    MotDePasse = HashPassword(model.MotDePasse)
                };

                _context.Comptes.Add(compte);
                await _context.SaveChangesAsync();

                return Ok(new { 
                    message = "Inscription réussie",
                    success = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'inscription: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return StatusCode(500, new { 
                    message = "Une erreur est survenue lors de l'inscription",
                    errors = new[] { "Veuillez réessayer plus tard" }
                });
            }
        }

        // Connexion
        [HttpPost("connexion")]
        public async Task<IActionResult> Connexion([FromBody] ConnexionModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var compte = await _context.Comptes.FirstOrDefaultAsync(c => c.Email == model.Email);

            if (compte == null || !VerifyPassword(model.MotDePasse, compte.MotDePasse))
                return Unauthorized(new { message = "Email ou mot de passe incorrect" });

            // Générer le JWT
            var token = GenerateJwtToken(compte);

            return Ok(new
            {
                message = "Connexion réussie",
                token = token,
                compte = new
                {
                    id = compte.id_compte,
                    nom = compte.Nom,
                    prenom = compte.Prenom,
                    email = compte.Email
                }
            });
        }

        private string GenerateJwtToken(Compte compte)
        {
            if (compte == null)
            {
                throw new ArgumentNullException(nameof(compte));
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, compte.id_compte.ToString()),
                new Claim(ClaimTypes.Email, compte.Email),
                new Claim(ClaimTypes.Name, $"{compte.Prenom} {compte.Nom}")
            };

            const string jwtKey = "MaCléUltraSecrèteEtLongue123456789!";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // Modèle pour Inscription
    public class InscriptionModel
    {
        [JsonPropertyName("nom")]
        [Required]
        public required string Nom { get; set; }

        [JsonPropertyName("prenom")]
        [Required]
        public required string Prenom { get; set; }

        [JsonPropertyName("email")]
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [JsonPropertyName("motDePasse")]
        [Required]
        [MinLength(6)]
        public required string MotDePasse { get; set; }
    }

    // Modèle pour Connexion
    public class ConnexionModel
    {
        public required string Email { get; set; }
        public required string MotDePasse { get; set; }
    }
}
