using Microsoft.EntityFrameworkCore;
using BackendGrenishop.DbContext;
using BackendGrenishop.Modeles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Grenishop API",
        Version = "v1",
        Description = "API pour la gestion des entreprises et des produits"
    });
});

// Configuration CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});

// Configuration de l'authentification JWT
var jwtKey = "MaCléUltraSecrèteEtLongue123456789!";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    Console.WriteLine($"Tentative de connexion à la base de données avec la chaîne : {connectionString}");
    try
    {
        options.UseSqlServer(connectionString);
        Console.WriteLine("Configuration de la base de données réussie");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur lors de la configuration de la base de données : {ex.Message}");
        Console.WriteLine($"Stack trace : {ex.StackTrace}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Erreur interne : {ex.InnerException.Message}");
        }
    }
});

var app = builder.Build();

// Ajouter des logs pour le débogage
Console.WriteLine("Environnement : " + app.Environment.EnvironmentName);
Console.WriteLine("URL de l'application : " + builder.Configuration["ASPNETCORE_URLS"]);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Mode développement activé");
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Grenishop API V1");
    });
}
else
{
    Console.WriteLine("Mode production activé");
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Grenishop API V1");
    });
}

// Activer CORS avant les autres middlewares
app.UseCors("AllowAll");

// Désactiver la redirection HTTPS
// app.UseHttpsRedirection();

// Appliquer les migrations automatiquement
try
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        Console.WriteLine("Tentative d'application des migrations...");
        
        // Vérifier les migrations en attente
        var pendingMigrations = context.Database.GetPendingMigrations();
        if (pendingMigrations.Any())
        {
            Console.WriteLine($"Migrations en attente : {string.Join(", ", pendingMigrations)}");
            context.Database.Migrate();
            Console.WriteLine("Migrations appliquées avec succès.");
        }
        else
        {
            Console.WriteLine("Aucune migration en attente.");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Erreur lors de l'application des migrations : {ex.Message}");
    Console.WriteLine($"Stack trace : {ex.StackTrace}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Erreur interne : {ex.InnerException.Message}");
    }
}



app.MapControllers();

// Route par défaut
app.MapGet("/", () => "Bienvenue sur l'API Grenishop!");

// Ajouter l'authentification au pipeline
app.UseAuthentication();
app.UseAuthorization();

Console.WriteLine("Application démarrée. Appuyez sur Ctrl+C pour arrêter.");
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
