using CustRewardMgtSys.Domain.Entities;
using CustRewardMgtSys.Domain.Enum;
using CustRewardMgtSys.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CustRewardMgtSys.Infrastructure
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();//http://localhost:60068/  http://localhost:59689/
            optionsBuilder.UseNpgsql("Host=82.29.173.232;Port=30007;Username=postgreadmin;Password=123456@Abc;Database=silkcoatDb");

            var context = new AppDbContext(optionsBuilder.Options);

            // Invoke the SeedData method to populate the database
            //silkcoatDb
            //silkcoaatDb

            SeedData(context);

            return context;
        }

        private static void SeedData(AppDbContext context)
        {
            // Ensure the database is created
            context.Database.EnsureCreated();
            //context.Database.Migrate();

            // Seed roles if they do not exist
            if (!context.Roles.Any())
            {
                var roles = new List<IdentityRole>
                {
                    new IdentityRole {Id="b1e1a2c3-d4f5-6789-abcd-ef0123456789", Name = "HeadAdmin", NormalizedName = "HEADADMIN" },
                    new IdentityRole {Id="c2f3b4d5-e6a7-8901-bcde-fa1234567890", Name = "Admin", NormalizedName = "ADMIN" },
                    new IdentityRole {Id="d3g4c5e6-f7b8-9012-cdef-ab2345678901", Name = "PaintBuyer", NormalizedName = "PAINTBUYER" },
                };

                context.Roles.AddRange(roles);
                context.SaveChanges();
            }

            // Seed users if they do not exist
            if (!context.Users.Any())
            {
                var hasher = new PasswordHasher<ApplicationUser>();

                var headUser = new ApplicationUser
                {
                    Id= "b1e1a2c3-d4f5-6789-abcd-ef0123456789",
                    UserType = USER_TYPE.ADMIN,
                    Gender = GENDER.MALE,
                    FirstName = "head",
                    LastName = "admin",
                    PhoneNumber = "",
                    UserName = "headadmin",
                    NormalizedUserName = "HEADADMIN",
                    State = "",
                    Email = "info@silkcoat.com.ng",
                    NormalizedEmail = "INFO@SILKCOAT.COM.NG",
                    Town = "",
                    EmailConfirmed = true,
                };
                headUser.PasswordHash = hasher.HashPassword(headUser, "123456@Abc");

                var adminUser = new ApplicationUser
                {
                    Id = "c2f3b4d5-e6a7-8901-bcde-fa1234567890",
                    UserType = USER_TYPE.ADMIN,
                    Gender = GENDER.MALE,
                    FirstName = "Silkcoat",
                    LastName = "admin",
                    PhoneNumber = "",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    State = "",
                    Email = "info@silkcoat.com.ng",
                    NormalizedEmail = "INFO@SILKCOAT.COM.NG",
                    Town = "",
                    EmailConfirmed = true,
                };
                adminUser.PasswordHash = hasher.HashPassword(adminUser, "123456@Abc");

                context.Users.AddRange(headUser, adminUser);
                context.SaveChanges();

                // Assign roles to users
                var userRoles = new List<IdentityUserRole<string>>
                {
                    new IdentityUserRole<string> { UserId = headUser.Id, RoleId = context.Roles.Single(r => r.Name == "HeadAdmin").Id },
                    new IdentityUserRole<string> { UserId = adminUser.Id, RoleId = context.Roles.Single(r => r.Name == "Admin").Id },
                };

                context.UserRoles.AddRange(userRoles);
                context.SaveChanges();
            }
        }
    }
}