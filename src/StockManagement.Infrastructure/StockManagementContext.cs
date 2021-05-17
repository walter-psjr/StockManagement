using Microsoft.EntityFrameworkCore;
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
            modelBuilder.Entity<Store>()
                .Property(x => x.Id)
                .HasColumnName("StoreId");

            modelBuilder.Entity<Product>()
                .Property(x => x.Id)
                .HasColumnName("ProductId");
        }
    }
}