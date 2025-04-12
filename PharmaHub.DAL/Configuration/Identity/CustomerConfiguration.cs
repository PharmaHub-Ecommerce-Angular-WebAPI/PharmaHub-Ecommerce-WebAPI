using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaHub.Domain.Entities.Identity;

namespace PharmaHub.DAL.Configuration.Identity
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            builder.Property(c=>c.Id)
                .IsRequired()
                .HasMaxLength(130);

            // Configure relationships

            //builder.HasMany(c => c.FavoriteProductsList)
            //    .WithOne(fp => fp.Customer)
            //    .HasForeignKey(fp => fp.CustomerId)
            //    .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade); 

        }
    }
}
