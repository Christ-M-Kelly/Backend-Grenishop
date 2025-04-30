using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendGrenishop.DbContext;
using BackendGrenishop.Modeles;
using Microsoft.Extensions.Logging;

namespace BackendGrenishop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestDataController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TestDataController> _logger;

    public TestDataController(ApplicationDbContext context, ILogger<TestDataController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("test-connection")]
    public async Task<IActionResult> TestConnection()
    {
        try
        {
            _logger.LogInformation("Tentative de connexion à la base de données...");
            
            // Tenter de se connecter à la base de données
            var canConnect = await _context.Database.CanConnectAsync();
            
            if (!canConnect)
            {
                _logger.LogError("Impossible de se connecter à la base de données");
                return BadRequest(new
                {
                    Status = "Error",
                    Message = "Impossible de se connecter à la base de données",
                    ConnectionString = _context.Database.GetConnectionString()
                });
            }

            // Vérifier si les tables existent
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
            
            _logger.LogInformation("Connexion réussie à la base de données");
            
            return Ok(new
            {
                Status = "Success",
                Message = "Connexion à la base de données réussie",
                DatabaseName = _context.Database.GetDbConnection().Database,
                ServerName = _context.Database.GetDbConnection().DataSource,
                PendingMigrations = pendingMigrations,
                ConnectionString = _context.Database.GetConnectionString()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la connexion à la base de données");
            return BadRequest(new
            {
                Status = "Error",
                Message = "Erreur de connexion à la base de données",
                Error = ex.Message,
                InnerError = ex.InnerException?.Message,
                StackTrace = ex.StackTrace
            });
        }
    }

    [HttpPost("initialize")]
    public async Task<IActionResult> InitializeTestData()
    {
        try
        {
            // Vérifier si des données existent déjà
            if (await _context.Marque.AnyAsync())
            {
                return BadRequest("Des données existent déjà dans la base de données.");
            }

            // Créer des marques
            var marque1 = new Marque { Nom = "Grenishop" };
            var marque2 = new Marque { Nom = "TechShop" };

            _context.Marque.AddRange(marque1, marque2);
            await _context.SaveChangesAsync();

            // Créer des modèles
            var modeles = new List<Modele>
            {
                new Modele
                {
                    nom_modele = "iPhone 13",
                    prix_neuf = 999.99m,
                    prix_occasion = 799.99m,
                    nbr_neuf = 10,
                    nbr_occasion = 5,
                    id_marque = marque1.id_marque
                },
                new Modele
                {
                    nom_modele = "Samsung Galaxy S21",
                    prix_neuf = 899.99m,
                    prix_occasion = 699.99m,
                    nbr_neuf = 8,
                    nbr_occasion = 3,
                    id_marque = marque1.id_marque
                },
                new Modele
                {
                    nom_modele = "MacBook Pro",
                    prix_neuf = 1299.99m,
                    prix_occasion = 999.99m,
                    nbr_neuf = 5,
                    nbr_occasion = 2,
                    id_marque = marque2.id_marque
                }
            };

            _context.Modele.AddRange(modeles);
            await _context.SaveChangesAsync();

            // Créer des produits
            var produits = new List<Produit>
            {
                new Produit
                {
                    Nom = "iPhone 13 Neuf",
                    Etat = "Neuf",
                    id_modele = modeles[0].id_modele
                },
                new Produit
                {
                    Nom = "iPhone 13 Occasion",
                    Etat = "Occasion",
                    id_modele = modeles[0].id_modele
                },
                new Produit
                {
                    Nom = "Samsung Galaxy S21 Neuf",
                    Etat = "Neuf",
                    id_modele = modeles[1].id_modele
                },
                new Produit
                {
                    Nom = "Samsung Galaxy S21 Occasion",
                    Etat = "Occasion",
                    id_modele = modeles[1].id_modele
                },
                new Produit
                {
                    Nom = "MacBook Pro Neuf",
                    Etat = "Neuf",
                    id_modele = modeles[2].id_modele
                },
                new Produit
                {
                    Nom = "MacBook Pro Occasion",
                    Etat = "Occasion",
                    id_modele = modeles[2].id_modele
                }
            };

            _context.Produits.AddRange(produits);
            await _context.SaveChangesAsync();

            return Ok("Données de test initialisées avec succès.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de l'initialisation des données de test");
            return BadRequest(new
            {
                Status = "Error",
                Message = "Erreur lors de l'initialisation des données de test",
                Error = ex.Message,
                InnerError = ex.InnerException?.Message
            });
        }
    }
} 