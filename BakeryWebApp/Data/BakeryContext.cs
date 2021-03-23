using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BakeryWebApp
{
    public partial class BakeryContext : DbContext
    {


        public BakeryContext(DbContextOptions<BakeryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DayPlan> DayPlans { get; set; }
        public virtual DbSet<DayProduction> DayProductions { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<Norm> Norms { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductsMaterial> ProductsMaterials { get; set; }
        public virtual DbSet<ProductsPlan> ProductsPlans { get; set; }
        public virtual DbSet<ProductsProduction> ProductsProductions { get; set; }
        public virtual DbSet<Raw> Raws { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<DayPlan>(entity =>
            {
                entity.Property(e => e.DayPlanId).HasColumnName("dayPlanId");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");
            });

            modelBuilder.Entity<DayProduction>(entity =>
            {
                entity.Property(e => e.DayProductionId).HasColumnName("dayProductionId");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.Property(e => e.MaterialId).HasColumnName("materialId");

                entity.Property(e => e.MaterialName)
                    .HasMaxLength(100)
                    .IsUnicode(true)
                    .HasColumnName("materialName");
            });

            modelBuilder.Entity<Norm>(entity =>
            {
                entity.Property(e => e.NormId).HasColumnName("normId");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.RowId).HasColumnName("rowId");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Norms)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__Norms__productId__33D4B598");

                entity.HasOne(d => d.Raw)
                    .WithMany(p => p.Norms)
                    .HasForeignKey(d => d.RowId)
                    .HasConstraintName("FK__Norms__rowId__32E0915F");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(true)
                    .HasColumnName("productName");
            });

            modelBuilder.Entity<ProductsMaterial>(entity =>
            {
                entity.HasKey(e => e.ProductMaterial)
                    .HasName("PK__Products__59C1381A7723A73A");

                entity.Property(e => e.ProductMaterial).HasColumnName("productMaterial");

                entity.Property(e => e.MaterialId).HasColumnName("materialId");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Material)
                    .WithMany(p => p.ProductsMaterials)
                    .HasForeignKey(d => d.MaterialId)
                    .HasConstraintName("FK__ProductsM__mater__34C8D9D1");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductsMaterials)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__ProductsM__produ__35BCFE0A");
            });

            modelBuilder.Entity<ProductsPlan>(entity =>
            {
                entity.HasKey(e => e.ProductPlan)
                    .HasName("PK__Products__0406D389B6375C7B");

                entity.Property(e => e.ProductPlan).HasColumnName("productPlan");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.DayPlanId).HasColumnName("dayPlanId");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.HasOne(d => d.DayPlan)
                    .WithMany(p => p.ProductsPlans)
                    .HasForeignKey(d => d.DayPlanId)
                    .HasConstraintName("FK__ProductsP__dayPl__36B12243");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductsPlans)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__ProductsP__produ__37A5467C");
            });

            modelBuilder.Entity<ProductsProduction>(entity =>
            {
                entity.HasKey(e => e.ProductProductionId)
                    .HasName("PK__Products__F95A733B5D7B838E");

                entity.Property(e => e.ProductProductionId).HasColumnName("productProductionId");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.DayProductionId).HasColumnName("dayProductionId");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.HasOne(d => d.DayProduction)
                    .WithMany(p => p.ProductsProductions)
                    .HasForeignKey(d => d.DayProductionId)
                    .HasConstraintName("FK__ProductsP__dayPr__38996AB5");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductsProductions)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__ProductsP__produ__398D8EEE");
            });

            modelBuilder.Entity<Raw>(entity =>
            {
                entity.ToTable("Raws");

                entity.Property(e => e.RawId).HasColumnName("rawId");

                entity.Property(e => e.RawName)
                    .HasMaxLength(100)
                    .IsUnicode(true)
                    .HasColumnName("rawName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
