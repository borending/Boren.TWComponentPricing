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
        public virtual DbSet<Promotion> Promotions { get; set; }
        public virtual DbSet<PromotionProduct> PromotionProducts { get; set; }
        public virtual DbSet<Raw> Raws { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese (Traditional)_Taiwan.950");

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand");

                entity.HasComment("品牌");

                entity.Property(e => e.Name).HasMaxLength(128);
            });

            modelBuilder.Entity<Categroy>(entity =>
            {
                entity.ToTable("Categroy");

                entity.HasComment("分類");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Detail>(entity =>
            {
                entity.ToTable("Detail");

                entity.HasComment("詳細價格");

                entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"Price_Id_seq\"'::regclass)");

                entity.Property(e => e.DateTime).HasColumnType("date");

                entity.Property(e => e.Price).HasPrecision(10, 2);

                entity.Property(e => e.Remarks).HasColumnType("character varying(255)[]");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Details)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("Detail_ProductId_fkey");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.HasComment("產品");

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

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.ToTable("Promotion");

                entity.HasComment("促銷");

                entity.Property(e => e.FixedText).HasMaxLength(128);

                entity.Property(e => e.OriginText).HasMaxLength(128);

                entity.HasOne(d => d.Detail)
                    .WithMany(p => p.Promotions)
                    .HasForeignKey(d => d.DetailId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Promotion_DetailId_fkey");
            });

            modelBuilder.Entity<PromotionProduct>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PromotionProduct");

                entity.HasComment("促銷關聯");

                entity.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("PromotionProduct_ProductId_fkey");

                entity.HasOne(d => d.Promotion)
                    .WithMany()
                    .HasForeignKey(d => d.PromotionId)
                    .HasConstraintName("PromotionProduct_PromotionId_fkey");
            });

            modelBuilder.Entity<Raw>(entity =>
            {
                entity.ToTable("Raw");

                entity.Property(e => e.CategroyId).HasComment("分類");

                entity.Property(e => e.Done).HasComment("是否解析完成");

                entity.Property(e => e.FixedText)
                    .HasMaxLength(255)
                    .HasComment("清洗之後的字");

                entity.Property(e => e.Time).HasComment("資料抓取的時間");

                entity.HasOne(d => d.Categroy)
                    .WithMany(p => p.Raws)
                    .HasForeignKey(d => d.CategroyId)
                    .HasConstraintName("Raw_CategroyId_fkey");

                entity.HasOne(d => d.RawNavigation)
                    .WithMany(p => p.InverseRawNavigation)
                    .HasForeignKey(d => d.RawId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Raw_RawId_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
