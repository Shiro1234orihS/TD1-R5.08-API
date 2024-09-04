using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TD1_code.Models.EntityFramework;

namespace TD1_code.Models.EntityFramework
{
    public partial class DBContexte : DbContext
    {
        public DBContexte()
        {
        }

        public DBContexte(DbContextOptions<DbContext> options):base(options) { }
        public virtual DbSet<Marque> Marques { get; set; }
        public virtual DbSet<Produit> Produits { get; set; }
        public virtual DbSet<TypeProduit> TypeProduits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produit>(entity =>
            {
                entity.HasKey(e => new { e.IdMarque, e.IdTypeProduit }).HasName("pk_produit");

                


                entity.HasOne(d => d.IdTypeProduitNavigation).WithMany(p => p.Produits)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_produit_typeProduit");

                entity.HasOne(d => d.IdMarqueNavigation).WithMany(p => p.Produits)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_produit_marque");
            });

            modelBuilder.Entity<Marque>(entity =>
            {
                entity.HasKey(e => e.IdMarque).HasName("pk_produit");
            });
            modelBuilder.Entity<TypeProduit>(entity =>
            {
                entity.HasKey(e => e.IdTypeProduit).HasName("pk_typeProduit");
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Server=localhost;port=5432;Database=SAR_BMW; uid=postgres; password=Ricardo2003@");
            }
        }

    }
}
