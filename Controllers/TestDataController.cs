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
            if (await _context.Entreprises.AnyAsync())
            {
                return BadRequest("Des données existent déjà dans la base de données.");
            }

            // Créer des entreprises
            var entreprise1 = new Entreprise { Nom_Entreprise = "Grenishop" };
            var entreprise2 = new Entreprise { Nom_Entreprise = "TechShop" };

            _context.Entreprises.AddRange(entreprise1, entreprise2);
            await _context.SaveChangesAsync();

            // Créer des produits
            var produits = new List<Produit>
            {
                new Produit
                {
                    Nom = "iPhone 13",
                    Etat = "Neuf",
                    EntrepriseID = entreprise1.EntrepriseID,
                    Nombre_Neuf = 10,
                    Nombre_Occasion = 5,
                    Prix_Neuf = 999.99m,
                    Prix_Occasion = 799.99m
                },
                new Produit
                {
                    Nom = "Samsung Galaxy S21",
                    Etat = "Neuf",
                    EntrepriseID = entreprise1.EntrepriseID,
                    Nombre_Neuf = 8,
                    Nombre_Occasion = 3,
                    Prix_Neuf = 899.99m,
                    Prix_Occasion = 699.99m
                },
                new Produit
                {
                    Nom = "MacBook Pro",
                    Etat = "Neuf",
                    EntrepriseID = entreprise2.EntrepriseID,
                    Nombre_Neuf = 5,
                    Nombre_Occasion = 2,
                    Prix_Neuf = 1299.99m,
                    Prix_Occasion = 999.99m
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