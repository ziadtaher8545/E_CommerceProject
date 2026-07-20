using E_Commerce.Domain.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data.Configurations
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.SubTotal).HasColumnType("decimal(8,2)");
            builder.HasMany(o => o.Items).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.OwnsOne(o => o.ShipToAddress);
            builder.Property(o => o.Status).HasConversion<string>();
        }
    }
}
