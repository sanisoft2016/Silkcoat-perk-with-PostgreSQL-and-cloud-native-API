using CustRewardMgtSys.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CustRewardMgtSys.Infrastructure.Data
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
       

        public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PinCode>()
                .HasOne(u => u.PinConsumption) // One User has one UserProfile
                .WithOne(up => up.Pin) // One UserProfile belongs to one User
                .HasForeignKey<PinConsumption>(up => up.PinId) // UserProfile's PK is also FK
                .IsRequired(); // Ensures it's mandatory

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.UserName)
                .HasMaxLength(50);
            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.NormalizedUserName)
                .HasMaxLength(50);

            modelBuilder.Entity<ApplicationUser>()
               .Property(u => u.Email)
               .HasMaxLength(50);

            modelBuilder.Entity<ApplicationUser>()
               .Property(u => u.NormalizedEmail)
               .HasMaxLength(50);


            modelBuilder.Entity<IdentityRole>()
                .Property(r => r.Name)
                .HasMaxLength(20);

            modelBuilder.Entity<IdentityRole>()
               .Property(r => r.NormalizedName)
               .HasMaxLength(20);

            modelBuilder.Entity<IdentityRole>()
               .Property(r => r.Id)
               .HasMaxLength(40);


            modelBuilder.Entity<IdentityUserRole<string>>()
              .Property(r => r.UserId)
              .HasMaxLength(40);
            modelBuilder.Entity<IdentityUserRole<string>>()
             .Property(r => r.RoleId)
             .HasMaxLength(40);
        }

        public DbSet<PinCode> PaintCategoryPins { get; set; }

        public DbSet<PinConsumption> PinConsumptions { get; set; }
        public DbSet<PaintMainCategory> PaintMainCategories { get; set; }
        public DbSet<PaintSubCategory> PaintSubCategories { get; set; }
    }
}
