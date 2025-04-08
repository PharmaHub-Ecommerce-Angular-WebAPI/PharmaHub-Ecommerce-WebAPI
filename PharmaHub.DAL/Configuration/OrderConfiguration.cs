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
            builder.HasKey(o => o.ID);
            builder.Property(o => o.ID)
                .HasColumnName("OrderID");


            builder.HasIndex(o => o.OrderDate)
                .HasDatabaseName("IX_Order_Date"); // Index on OrderDate


            builder.Property(o => o.PaymentMethod)
                .IsRequired()
                .HasConversion<string>(); // Store as string in the database

        }
    }
}
