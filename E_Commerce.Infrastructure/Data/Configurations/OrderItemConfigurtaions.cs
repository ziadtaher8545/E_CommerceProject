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
    internal class OrderItemConfigurtaions : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(oi => oi.Price).HasColumnType("decimal(8,2)");
            builder.OwnsOne(o => o.Product);
        }
    }
}
