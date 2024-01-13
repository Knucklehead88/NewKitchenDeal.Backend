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
            builder.HasMany(b => b.Locations).WithOne().OnDelete(DeleteBehavior.Cascade);    
        }
    }
}
