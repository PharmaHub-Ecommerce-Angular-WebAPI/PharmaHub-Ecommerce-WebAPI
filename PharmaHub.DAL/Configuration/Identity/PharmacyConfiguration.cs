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
    public class PharmacyConfiguration : IEntityTypeConfiguration<Pharmacy>
    {
        public void Configure(EntityTypeBuilder<Pharmacy> builder)
        {
            builder.ToTable("Pharmacies");

            builder.Property(p => p.PharmacyName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.AllowCreditCard)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(p => p.CreditCardNumber)
                .HasMaxLength(25)
                .IsRequired(false);

            builder.Property(p => p.OpenTime)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(p => p.CloseTime)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(p => p.FormalPapersURL)
                .IsRequired(false);

            builder.Property(p => p.LogoURL)
                .IsRequired()
                .HasDefaultValue("https://i.pinimg.com/736x/ce/f1/2d/cef12d52c6ceb1076c1812b4560c05d1.jpg");
        }
    }
}
