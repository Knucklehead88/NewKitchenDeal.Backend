using Core.Entities;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Infrastructure.Data.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        public DbSet<PersonalInfo> PersonalInfo { get; set; }
        public DbSet<BusinessInfo> BusinessInfo { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Language> Languages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PersonalInfo>().ToTable("PersonalInfo");
            builder.Entity<BusinessInfo>().ToTable("BusinessInfo");

        }
    }
}