using Microsoft.EntityFrameworkCore;
using BackendGrenishop.Modeles;

namespace BackendGrenishop.DbContext;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Entreprise> Entreprises { get; set; }
    public DbSet<Produit> Produits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Produit>()
            .Property(p => p.Prix_Neuf)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Produit>()
            .Property(p => p.Prix_Occasion)
            .HasPrecision(10, 2);
    }
}
