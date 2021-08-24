using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Boren.TWComponentPricing.Data
{
    public partial class PricingDbContext : DbContext
    {
        public PricingDbContext()
        {
        }

        public PricingDbContext(DbContextOptions<PricingDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Categroy> Categroys { get; set; }
        public virtual DbSet<Detail> Details { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese (Traditional)_Taiwan.950");

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand");

                entity.Property(e => e.Name).HasMaxLength(128);
            });

            modelBuilder.Entity<Categroy>(entity =>
            {
                entity.ToTable("Categroy");

                entity.Property(e => e.Name).HasMaxLength(128);
            });

            modelBuilder.Entity<Detail>(entity =>
            {
                entity.ToTable("Detail");

                entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"Price_Id_seq\"'::regclass)");

                entity.Property(e => e.DateTime).HasColumnType("date");

                entity.Property(e => e.Price).HasPrecision(10, 2);

                entity.Property(e => e.Remarks).HasColumnType("character varying[]");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Details)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("Detail_ProductId_fkey");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.FixedText).HasMaxLength(128);

                entity.Property(e => e.OriginText)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Product_BrandId_fkey");

                entity.HasOne(d => d.Categroy)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategroyId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Product_CategroyId_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
