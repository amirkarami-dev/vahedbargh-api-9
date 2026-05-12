namespace Coreapi.Infrastructure.Notification;


using Microsoft.EntityFrameworkCore;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) { }

    public DbSet<UserConnection> UserConnections { get; set; }
    public DbSet<UserToken> UserTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserConnection>()
            .HasKey(uc => uc.UserId);
        modelBuilder.Entity<UserToken>()
            .HasKey(ut => new { ut.UserId, ut.Token }); // Composite key
    }
}