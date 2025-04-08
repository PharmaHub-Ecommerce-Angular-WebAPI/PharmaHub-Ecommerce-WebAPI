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
    internal class SuggestedMedicineConfiguration : IEntityTypeConfiguration<SuggestedMedicine>
    {
        public void Configure(EntityTypeBuilder<SuggestedMedicine> builder)
        {
            builder.HasKey(sm => sm.Id);
            builder.Property(sm => sm.Name)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnType("nvarchar(150)");
            builder.HasIndex(sm => sm.Name)
                .HasDatabaseName("IX_SuggestedMedicine_Name");

            builder.Property(sm => sm.Strength)
                .IsRequired()
                .HasColumnType("smallint")
                .HasDefaultValue(0);

        }
    }
}
