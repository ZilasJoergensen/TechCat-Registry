using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using TechCatRegistry.Core;

namespace TechCatRegistry.Data
{
    public class TechCatDbContext : DbContext
    {
        public DbSet<ParamGroup> ParameterGroup { get; set; }
        public DbSet<TechCatRegistry.Core.Parameter> Parameter { get; set; }
        public DbSet<DataPoint> DataPoint { get; set; }
        public DbSet<Catalog> Catalog { get; set; }
        public DbSet<Component> Component { get; set; }

        public TechCatDbContext(DbContextOptions<TechCatDbContext> options) : base(options)
        {
        }

        // https://learn.microsoft.com/en-us/ef/core/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DataPoint>(entity =>
            {
                entity.HasIndex(d => new { d.ComponentId, d.ParameterId, d.Year, d.Estimate }).IsUnique();

                entity.Property(d => d.Estimate)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(d => d.TxtValue).HasMaxLength(50);

                entity.Property(d => d.NumericValue).HasPrecision(18, 6);

                entity.ToTable(t =>
                {
                    // https://learn.microsoft.com/en-us/ef/core/modeling/indexes?tabs=data-annotations#check-constraints
                    t.HasCheckConstraint("CK_DataPoint_ExactlyOneValue", "([NumericValue] IS NULL) <> ([TxtValue] IS NULL)");
                    t.HasCheckConstraint("CK_DataPoint_Estimate", "[Estimate] IN ('ctrl', 'lower', 'upper')");
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

                entity.HasIndex(p => p.Name).IsUnique();
            });

            modelBuilder.Entity<Catalog>(entity =>
            {
                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(c => c.PublishedOn).HasColumnType("date");
            });

            modelBuilder.Entity<ParamGroup>(entity =>
            {
                entity.Property(g => g.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });
        }
    }
}
