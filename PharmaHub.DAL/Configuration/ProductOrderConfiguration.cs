using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaHub.Domain.Entities;

namespace PharmaHub.DAL.Configuration
{
    public class ProductOrderConfiguration : IEntityTypeConfiguration<ProductOrder>
    {
        public void Configure(EntityTypeBuilder<ProductOrder> builder)
        {
            builder.ToTable("ProductOrders"); 

            builder.HasKey(po => new { po.ProductId, po.OrderId });
            builder.Property(po => po.Amount)
                .IsRequired()
                .HasDefaultValue(1); // Default value for Amount


            // Configure relationships
            builder.HasOne(builder => builder.Product)
                .WithMany(product => product.ProductOrdersList)
                .HasForeignKey(po => po.ProductId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.HasOne(builder => builder.Order)
                .WithMany(order => order.ProductOrdersList)
                .HasForeignKey(po => po.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

        }

    }
}