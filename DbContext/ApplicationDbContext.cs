using Microsoft.EntityFrameworkCore;
using BackendGrenishop.Modeles;

namespace BackendGrenishop.DbContext;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Marque> Marque { get; set; }
    public DbSet<Modele> Modele { get; set; }
    public DbSet<Produit> Produits { get; set; }
    public DbSet<Compte> Comptes { get; set; }
    public DbSet<Commande> Commandes { get; set; }
    public DbSet<ListeDeSouhaits> ListeDeSouhaits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Modele>()
            .Property(p => p.prix_neuf)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Modele>()
            .Property(p => p.prix_occasion)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Compte>()
            .HasIndex(c => c.Email)
            .IsUnique();

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

        modelBuilder.Entity<Compte>()
            .HasMany(c => c.Commandes)
            .WithOne(c => c.Compte)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Compte>()
            .HasMany(c => c.ListeDeSouhaits)
            .WithOne(l => l.Compte)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Commande>()
            .HasMany(c => c.Produits)
            .WithOne(p => p.Commande)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
