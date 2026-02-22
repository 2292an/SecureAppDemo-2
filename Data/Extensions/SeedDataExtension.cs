using Microsoft.EntityFrameworkCore;
using SecureAppDemo.Data.Context;
using SecureAppDemo.Data.Entities;

namespace SecureAppDemo.Data.Extensions;

public static class SeedData
{
    public static async Task SeedDataAsync(this IServiceProvider serviceProvider, IConfigurationSection configuration)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        var defaultAdminPwd = configuration["DefaultAdminPwd"]!;

        await context.Database.MigrateAsync();

        if (await context.Users.AnyAsync())
            return; // seed data already exists

        var adminRole = new Role
        {
            Name = "Admin"
        };

        var dataWarehouseOperatorRole = new Role
        {
            Name = "DataWarehouseOperator"
        };

        context.Roles.AddRange(adminRole, dataWarehouseOperatorRole);

        var adminUser = new User
        {
            Username = "admin",
            ExternalId = Guid.NewGuid(),
            Email = "admin@test.com",
            FirstName = "Angelica",
            LastName = "Ulate",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(defaultAdminPwd)
        };

        context.Users.Add(adminUser);

        var userRoles = new List<UserRole>
        {
            new() {
                User = adminUser,
                Role = adminRole
            },
        };

        context.UserRoles.AddRange(userRoles);
        await context.SaveChangesAsync();
    }
}