using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BackendGrenishop.Modeles;
using BackendGrenishop.Models;

namespace BackendGrenishop.DbContext;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Marque> Marque { get; set; }
    public DbSet<Modele> Modele { get; set; }
    public DbSet<Produit> Produits { get; set; }
    public DbSet<Commande> Commandes { get; set; }
    public DbSet<ListeDeSouhaits> ListeDeSouhaits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Call base method to configure Identity tables
        base.OnModelCreating(modelBuilder);

        // Configure decimal precision for Modele
        modelBuilder.Entity<Modele>()
            .Property(p => p.prix_neuf)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Modele>()
            .Property(p => p.prix_occasion)
            .HasPrecision(10, 2);

        // Configure indexes
        modelBuilder.Entity<Marque>()
            .HasIndex(m => m.Nom)
            .IsUnique();

        modelBuilder.Entity<Modele>()
            .HasIndex(m => m.nom_modele);

        modelBuilder.Entity<Produit>()
            .HasIndex(p => p.Etat);

        modelBuilder.Entity<Commande>()
            .HasIndex(c => c.status_commande);

        modelBuilder.Entity<Commande>()
            .HasIndex(c => c.date_commande);

        // Configure relationships
        modelBuilder.Entity<Marque>()
            .HasMany(m => m.Modeles)
            .WithOne(m => m.Marque)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Modele>()
            .HasMany(m => m.Produits)
            .WithOne(p => p.Modele)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Modele>()
            .HasMany(m => m.ListeDeSouhaits)
            .WithOne(l => l.Modele)
            .OnDelete(DeleteBehavior.Cascade);

        // ApplicationUser relationships
        modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.Commandes)
            .WithOne(c => c.ApplicationUser)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.ListeDeSouhaits)
            .WithOne(l => l.ApplicationUser)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Commande>()
            .HasMany(c => c.Produits)
            .WithOne(p => p.Commande)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
