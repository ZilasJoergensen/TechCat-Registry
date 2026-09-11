using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using TechCatRegistry.Core;

namespace TechCatRegistry.Data
{
    public class TechCatDbContext : DbContext
    {
        public DbSet<ParameterGroup> ParameterGroup { get; set; }
        public DbSet<TechCatRegistry.Core.Parameter> Parameter { get; set; }
        public DbSet<DataPoint> DataPoint { get; set; }
        public DbSet<Catalog> Catalog { get; set; }
        public DbSet<Component> Component { get; set; }

        public TechCatDbContext(DbContextOptions<TechCatDbContext> options) : base(options)
        {
        }

        // Brugte den her som reference https://learn.microsoft.com/en-us/ef/core/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DataPoint>(entity =>
            {
                entity.HasIndex(d => new { d.ComponentId, d.ParameterId, d.EstimateTypeId, d.Year }).IsUnique();

                entity.Property(d => d.EstimateTypeId)
                    .IsRequired();

                entity.Property(d => d.TxtValue).HasMaxLength(50);

                entity.Property(d => d.NumericValue).HasPrecision(18, 6);

                // Havde problemer med at bruge det her, men her er referencen: https://learn.microsoft.com/en-us/ef/core/modeling/indexes?tabs=data-annotations#check-constraints
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_DataPoint_ExactlyOneValue", "([NumericValue] IS NULL AND [TxtValue] IS NOT NULL) OR ([TxtValue] IS NULL AND [NumericValue] IS NOT NULL)");
                });
            });

            
            modelBuilder.Entity<Component>(entity =>
            {
                entity.Property(c => c.SheetCode)
                    .IsRequired()
                    .HasMaxLength(31);

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(c => c.Catalog)
                    .WithMany(c => c.Components)
                    .HasForeignKey(c => c.CatalogId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(c => new { c.CatalogId, c.SheetCode }).IsUnique();
            });

            modelBuilder.Entity<TechCatRegistry.Core.Parameter>(entity =>
            {
                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasIndex(p => new { p.GroupId, p.Name }).IsUnique();
            });

            modelBuilder.Entity<Catalog>(entity =>
            {
                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(c => c.PublishedOn).HasColumnType("date");
            });

            modelBuilder.Entity<ParameterGroup>(entity =>
            {
                entity.Property(g => g.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<EstimateType>(entity =>
            {
                entity.Property(e => e.EstimateCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasData(
                    new EstimateType { EstimateTypeId = 1, EstimateCode = "ctrl" },
                    new EstimateType { EstimateTypeId = 2, EstimateCode = "lower" },
                    new EstimateType { EstimateTypeId = 3, EstimateCode = "upper" }
                    );
            });
        }
    }
}
