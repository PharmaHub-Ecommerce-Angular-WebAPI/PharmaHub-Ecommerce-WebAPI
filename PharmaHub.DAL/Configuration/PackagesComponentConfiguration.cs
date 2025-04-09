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
    public class PackagesComponentConfiguration : IEntityTypeConfiguration<PackagesComponent>
    {
        public void Configure(EntityTypeBuilder<PackagesComponent> builder)
        {
            builder.ToTable("PackagesComponents"); 

            builder.HasKey(pc => new { pc.ProductId, pc.ComponentName });

            builder.Property(pc => pc.ComponentName)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(pc => pc.Product)
                .WithMany(p => p.PackagesComponents)
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.Cascade); 



        }
    }

}
