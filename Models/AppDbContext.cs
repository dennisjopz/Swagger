using Microsoft.EntityFrameworkCore;
using Swagger.Models;
using static System.Net.Mime.MediaTypeNames;

public class AppDbContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }
    public DbSet<Images> Images { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>().HasKey(a => a.UserId);
        modelBuilder.Entity<Account>().HasAlternateKey(a => a.Id);

        modelBuilder.Entity<UserPermission>().HasKey(a => a.PermissionId);
        modelBuilder.Entity<UserPermission>().HasOne(a => a.Account).WithMany(a => a.UserPermissions).HasForeignKey(a => a.UserId);
        modelBuilder.Entity<UserPermission>().HasOne(a => a.Module).WithMany(a => a.UserPermissions).HasForeignKey(a => a.ModuleId);

        modelBuilder.Entity<Module>().HasKey(a=>a.ModuleId);

        modelBuilder.Entity<Images>().HasKey(image => image.ImageId);
        modelBuilder.Entity<Images>().HasOne(image => image.account).WithMany(account => account.Images).HasForeignKey(image => image.UserId);

        #region Seed Admin
        modelBuilder.Entity<Account>().HasData(
            new Account
            {
                Id = 1,
                UserId = "UID001",
                Username = "admin",
                Password = "AQAAAAIAAYagAAAAEMhlvX9lLfkx4sJ+6KR3zpMHt0pC9a4JvHqXjIrTei6e/iT35o8hdzIcOsoQa/VLNw==",
                Email = "admin@admin.com",
                IsAdmin = true,
                Status = true,
                AccountCreated = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }
        );
        #endregion
    }
}


