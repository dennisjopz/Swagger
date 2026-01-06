using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>().HasKey(a => a.UserId);
        modelBuilder.Entity<Account>().HasAlternateKey(a => a.Id);

        modelBuilder.Entity<UserPermission>().HasKey(a => a.PermissionId);
        modelBuilder.Entity<UserPermission>().HasOne(a => a.Account).WithMany(a => a.UserPermissions).HasForeignKey(a => a.UserId);
        modelBuilder.Entity<UserPermission>().HasOne(a => a.Module).WithMany(a => a.UserPermissions).HasForeignKey(a => a.ModuleId);

        modelBuilder.Entity<Module>().HasKey(a=>a.ModuleId);

        #region Seed Admin
        modelBuilder.Entity<Account>().HasData(
            new Account
            {
                Id = 1,
                UserId = "UID001",
                Username = "admin",
                Password = "password123",
                Email = "email@email.com",
                IsAdmin = true,
                Status = true,
                AccountCreated = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }
        );
        #endregion
    }
}


