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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.ID);
            builder.Property(o => o.ID)
                .HasColumnName("OrderID");


            builder.HasIndex(o => o.OrderDate)
                .HasDatabaseName("IX_Order_Date"); 


            builder.Property(o => o.PaymentMethod)
                .IsRequired()
                .HasConversion<string>();

            // Configure relationships
            builder.HasMany(o => o.ProductOrdersList)
                .WithOne(po => po.Order)
                .HasForeignKey(po => po.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // convert enum to string in data base 
            builder.Property(p => p.OrderStatus)
               .HasConversion<string>();

           
        }
    }
}
