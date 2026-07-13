using Microsoft.EntityFrameworkCore;
using T3TeknolojiAtolyeleri.Domain.Entities;
using T3TeknolojiAtolyeleri.Domain.Enums;
using T3TeknolojiAtolyeleri.Persistance.Context;
using T3TeknolojiAtolyeleriApplication.Interfaces.Security;

namespace T3TeknolojiAtolyeleri.API.SeedData
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasherService>();

            var adminExists = await context.AppUsers
                .AnyAsync(x => x.Role == UserRole.Admin);

            if (adminExists)
            {
                return;
            }

            var adminUser = new AppUser
            {
                FullName = "System Admin",
                Email = "admin@test.com",
                PasswordHash = passwordHasher.HashPassword("123456"),
                Role = UserRole.Admin,
                IsActive = true
            };

            await context.AppUsers.AddAsync(adminUser);
            await context.SaveChangesAsync();
        }
    }
}
