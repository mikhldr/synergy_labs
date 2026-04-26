using Microsoft.EntityFrameworkCore;
using Part3_WebAPI.Models;

namespace Part3_WebAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<User>().HasIndex(u => u.Login).IsUnique();
    }
}
