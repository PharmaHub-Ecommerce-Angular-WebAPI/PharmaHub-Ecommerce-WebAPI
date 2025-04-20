using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Enums;

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
                .IsRequired()     // (-1)Allow null for infinite stock
                .HasDefaultValue(1); // Null-able to allow for Infinite stock



            builder.Property(p => p.ImageUrl)
                .IsRequired(false)
                .HasDefaultValue(null); // Default to null for infinite stock


            builder.Property(p => p.DiscountRate)
                .IsRequired()
                .HasDefaultValue(0); // Send 0 if not provided (To allow front apply there PIpe line Depend on this value)


            builder.Property(p => p.Strength)
                .IsRequired(false);



            builder.HasIndex(p => p.Category)
                .HasDatabaseName("IX_Product_Category"); // Index on Category
            builder.Property(p => p.Category)
                .IsRequired()
                 .HasConversion(
                  v => v.ToString(), // write: enum -> string
                  v => (ProductCategory)Enum.Parse(typeof(ProductCategory), v)); // read: string -> enum; // ProductCategory is an enum, convert it to string for storage


            ///////// Configure relationships
            builder.HasMany(po => po.ProductOrdersList)
                .WithOne(o => o.Product)
                .HasForeignKey(po => po.ProductId)
                .HasConstraintName("FK_ProductOrder_ProductId")
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete for ProductOrders when Product is deleted


            builder.HasMany(p=>p.FavoriteProductsList)
                .WithOne(fp=>fp.Product)
                .HasForeignKey(fp => fp.ProductId)
                .HasConstraintName("FK_FavoriteProduct_ProductId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Pharmacy)
                .WithMany(ph => ph.productsList)
                .HasForeignKey(p => p.PharmacyId)
                .HasConstraintName("FK_Product_PharmacyId")
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(p => p.PackagesComponents)
                .WithOne(pc => pc.Product)
                .HasForeignKey(pc => pc.ProductId)
                .HasConstraintName("FK_PackagesComponent_ProductId")
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
