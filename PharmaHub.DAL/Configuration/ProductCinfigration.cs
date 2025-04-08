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
    public class ProductCinfigration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products"); 

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnName("ProductId"); // Rename Id to ProductId in the database


            builder.HasIndex(p => p.Name)
                .HasDatabaseName("IX_Product_Name"); // Unique index on Name



            builder.Property(p => p.Description)
                .HasMaxLength(150)
                .HasColumnName("ProductDescription");


            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            builder.HasIndex(p => p.Price)
                .HasDatabaseName("IX_Product_Price"); // Index on Price


            builder.Property(p => p.Quantity)
                .IsRequired(false)     // Allow null for infinite stock
                .HasDefaultValue(null); // Null-able to allow for Infinite stock



            builder.Property(p => p.ImageUrl)
                .IsRequired(false)
                .HasDefaultValue(null); // Default to null for infinite stock


            builder.Property(p => p.DiscountRate)
                .IsRequired(false)
                .HasDefaultValue(0); // Send 0 if not provided (To allow front apply there PIpe line Depend on this value)


            builder.Property(p => p.Strength)
                .IsRequired(false);



            builder.HasIndex(p => p.Category)
                .HasDatabaseName("IX_Product_Category"); // Index on Category
            builder.Property(p => p.Category)
                .IsRequired()
                .HasConversion<string>(); // ProductCategory is an enum, convert it to string for storage
        }
    }
}
