using Microsoft.EntityFrameworkCore;

namespace Db.Half.Models;

public class HalfContext : DbContext
{
    public HalfContext(DbContextOptions<HalfContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x => x.Login).IsUnique();
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<UserRoom> UserRooms { get; set; }
}