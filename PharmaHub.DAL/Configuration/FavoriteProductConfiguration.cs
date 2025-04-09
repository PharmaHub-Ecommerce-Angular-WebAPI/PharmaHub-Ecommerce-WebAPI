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
    public class FavoriteProductConfiguration : IEntityTypeConfiguration<FavoriteProduct>
    {
        public void Configure(EntityTypeBuilder<FavoriteProduct> builder)
        {

            builder.ToTable("FavoriteProducts");
            // Configure primary key
            builder.HasKey(fp => new { fp.CustomerId, fp.ProductId });


            // Configure relationships
            builder.HasOne(fp => fp.Customer)
                .WithMany(c => c.FavoriteProductsList)
                .HasForeignKey(fp => fp.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(fp => fp.Product)
                .WithMany(p => p.FavoriteProductsList)
                .HasForeignKey(fp => fp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
