using Microsoft.EntityFrameworkCore;
using SecureAppDemo.Data.Entities;

namespace SecureAppDemo.Data.Context;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User table
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();

         modelBuilder.Entity<User>()
            .HasIndex(u => u.ExternalId)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(u => u.ExternalId)
            .HasDefaultValueSql("NEWID()");

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        // UserRole table
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.Roles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        // Role table
        modelBuilder.Entity<Role>()
            .HasKey(r => r.Id);

         modelBuilder.Entity<Role>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();
    }
}