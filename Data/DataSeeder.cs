using BackendGrenishop.DbContext;
using BackendGrenishop.Modeles;
using BackendGrenishop.Models;
using Microsoft.AspNetCore.Identity;

namespace BackendGrenishop.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var logger = serviceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        logger.LogInformation("Seeding InMemory database with demo data...");

        // === Marques ===
        var marques = new List<Marque>
        {
            new() { id_marque = 1, Nom = "Apple" },
            new() { id_marque = 2, Nom = "Samsung" },
            new() { id_marque = 3, Nom = "Xiaomi" },
            new() { id_marque = 4, Nom = "Google" }
        };

        context.Marque.AddRange(marques);
        await context.SaveChangesAsync();

        // === Modèles ===
        var modeles = new List<Modele>
        {
            // Apple
            new() { id_modele = 1, nom_modele = "iPhone 15 Pro", nbr_neuf = 10, nbr_occasion = 5, prix_neuf = 1199.99m, prix_occasion = 899.99m, Tag = "Populaire", id_marque = 1 },
            new() { id_modele = 2, nom_modele = "iPhone 14", nbr_neuf = 8, nbr_occasion = 12, prix_neuf = 799.99m, prix_occasion = 549.99m, Tag = null, id_marque = 1 },
            // Samsung
            new() { id_modele = 3, nom_modele = "Galaxy S24 Ultra", nbr_neuf = 7, nbr_occasion = 3, prix_neuf = 1359.99m, prix_occasion = 999.99m, Tag = "Nouveau", id_marque = 2 },
            new() { id_modele = 4, nom_modele = "Galaxy A54", nbr_neuf = 20, nbr_occasion = 15, prix_neuf = 449.99m, prix_occasion = 299.99m, Tag = "Bon rapport qualité/prix", id_marque = 2 },
            // Xiaomi
            new() { id_modele = 5, nom_modele = "Xiaomi 14", nbr_neuf = 12, nbr_occasion = 6, prix_neuf = 899.99m, prix_occasion = 649.99m, Tag = null, id_marque = 3 },
            new() { id_modele = 6, nom_modele = "Redmi Note 13 Pro", nbr_neuf = 25, nbr_occasion = 10, prix_neuf = 299.99m, prix_occasion = 199.99m, Tag = "Best-seller", id_marque = 3 },
            // Google
            new() { id_modele = 7, nom_modele = "Pixel 8 Pro", nbr_neuf = 6, nbr_occasion = 4, prix_neuf = 1099.99m, prix_occasion = 799.99m, Tag = "Photo", id_marque = 4 },
            new() { id_modele = 8, nom_modele = "Pixel 7a", nbr_neuf = 15, nbr_occasion = 8, prix_neuf = 509.99m, prix_occasion = 349.99m, Tag = null, id_marque = 4 }
        };

        context.Modele.AddRange(modeles);
        await context.SaveChangesAsync();

        // === Produits ===
        var produits = new List<Produit>
        {
            // Produits disponibles (pas de commande)
            new() { id_produit = 1, Nom = "iPhone 15 Pro 256Go Noir", Etat = "Neuf", id_modele = 1 },
            new() { id_produit = 2, Nom = "iPhone 15 Pro 128Go Blanc", Etat = "Occasion", id_modele = 1 },
            new() { id_produit = 3, Nom = "iPhone 14 128Go Bleu", Etat = "Neuf", id_modele = 2 },
            new() { id_produit = 4, Nom = "Galaxy S24 Ultra 512Go Noir", Etat = "Neuf", id_modele = 3 },
            new() { id_produit = 5, Nom = "Galaxy A54 128Go Violet", Etat = "Occasion", id_modele = 4 },
            new() { id_produit = 6, Nom = "Xiaomi 14 256Go Noir", Etat = "Neuf", id_modele = 5 },
            new() { id_produit = 7, Nom = "Redmi Note 13 Pro 128Go Bleu", Etat = "Neuf", id_modele = 6 },
            new() { id_produit = 8, Nom = "Redmi Note 13 Pro 128Go Noir", Etat = "Occasion", id_modele = 6 },
            new() { id_produit = 9, Nom = "Pixel 8 Pro 256Go Noir", Etat = "Neuf", id_modele = 7 },
            new() { id_produit = 10, Nom = "Pixel 7a 128Go Blanc", Etat = "Occasion", id_modele = 8 },
            // Produits qui seront liés aux commandes (ajoutés ci-dessous)
            new() { id_produit = 11, Nom = "Galaxy S24 Ultra 256Go Gris", Etat = "Neuf", id_modele = 3 },
            new() { id_produit = 12, Nom = "iPhone 14 256Go Rouge", Etat = "Occasion", id_modele = 2 },
        };

        context.Produits.AddRange(produits);
        await context.SaveChangesAsync();

        // === Utilisateur de test ===
        const string testEmail = "test@grenishop.com";
        const string testPassword = "Test123!";

        var testUser = new ApplicationUser
        {
            UserName = testEmail,
            Email = testEmail,
            Nom = "Dupont",
            Prenom = "Jean",
            DateInscription = DateTime.UtcNow,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(testUser, testPassword);

        if (result.Succeeded)
        {
            logger.LogInformation("Test user created: {Email} / {Password}", testEmail, testPassword);

            // === Commandes ===
            var commandes = new List<Commande>
            {
                new()
                {
                    id_commande = 1,
                    date_commande = DateTime.UtcNow.AddDays(-5),
                    status_commande = "En attente",
                    adresse_livraison = "12 Rue de la Paix, 75002 Paris",
                    prix_total = 1359.99m,
                    UserId = testUser.Id
                },
                new()
                {
                    id_commande = 2,
                    date_commande = DateTime.UtcNow.AddDays(-15),
                    date_reception = DateTime.UtcNow.AddDays(-10),
                    status_commande = "Livrée",
                    adresse_livraison = "45 Avenue des Champs-Élysées, 75008 Paris",
                    prix_total = 549.99m,
                    UserId = testUser.Id
                }
            };

            context.Commandes.AddRange(commandes);
            await context.SaveChangesAsync();

            // Link produits to commandes
            var produit11 = await context.Produits.FindAsync(11);
            var produit12 = await context.Produits.FindAsync(12);

            if (produit11 != null) produit11.id_commande = 1;
            if (produit12 != null) produit12.id_commande = 2;

            await context.SaveChangesAsync();
        }
        else
        {
            logger.LogWarning("Failed to create test user: {Errors}",
                string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        logger.LogInformation("InMemory database seeded successfully!");
        logger.LogInformation("Test account: {Email} / {Password}", testEmail, testPassword);
    }
}
