using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StockManagement.Domain.Entities;

namespace StockManagement.Infrastructure
{
    public class StockManagementContext : DbContext
    {
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }

        public StockManagementContext(DbContextOptions<StockManagementContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>(entity =>
            {
                entity
                    .Property(x => x.Id)
                    .HasColumnName("StoreId");

                entity
                    .HasMany(x => x.StockItems)
                    .WithOne()
                    .IsRequired()
                    .HasForeignKey(x => x.StoreId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Product>()
                .Property(x => x.Id)
                .HasColumnName("ProductId");

            modelBuilder.Entity<StockItem>(entity =>
            {
                entity.HasKey(x => new {x.StoreId, x.ProductId});

                entity.UseXminAsConcurrencyToken();
            });
        }
    }
}