using Core.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Config
{
    public class BusinessInfoConfiguration : IEntityTypeConfiguration<BusinessInfo>
    {
        public void Configure(EntityTypeBuilder<BusinessInfo> builder)
        {

            builder
                .HasMany(b => b.Trades)
                .WithOne(b => b.BusinessInfo)
                .HasForeignKey(b => b.BusinessInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(b => b.SpokenLanguages)
                .WithOne(b => b.BusinessInfo)
                .HasForeignKey(b => b.BusinessInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(b => b.Locations)
                .WithOne(b => b.BusinessInfo)
                .HasForeignKey(b => b.BusinessInfoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
