using Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Config
{
    public class PersonalInfoConfiguration : IEntityTypeConfiguration<PersonalInfo>
    {
        public void Configure(EntityTypeBuilder<PersonalInfo> builder)
        {
            //builder
            //    .HasOne(p => p.Location)
            //    .WithOne(p => p.PersonalInfo)
            //    .HasForeignKey<Location>(p => p.PersonalInfoId)
            //    .IsRequired(false)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
