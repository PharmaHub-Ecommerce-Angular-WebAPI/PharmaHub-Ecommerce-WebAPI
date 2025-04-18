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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.Property(u => u.Country)
                .HasConversion<string>(); 

            builder.Property(u => u.city)
                .HasConversion<string>(); 

            builder.Property(u => u.AccountStat)
                .HasConversion<string>();

            builder.Property(u => u.Address)
                .HasMaxLength(300); // Set a maximum length for the address field

            builder.Property(u=>u.TrustScore)
                .HasColumnType("TINYINT") 
                .HasDefaultValue(100); // Each account Stats will have a max trust score of 100

            builder.HasIndex(u => u.PhoneNumber)
               .IsUnique();

        }

    }
}
