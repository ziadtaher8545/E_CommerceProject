using E_Commerce.Domain.Models.Orders;
using E_Commerce.Domain.Models.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data.DBContexts
{
    internal class StoreDBContext : DbContext
    {
        public StoreDBContext(DbContextOptions<StoreDBContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreDBContext).Assembly);
        }
    }
}
