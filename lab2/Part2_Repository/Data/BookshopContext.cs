using Microsoft.EntityFrameworkCore;
using Part2_Repository.Entities;

namespace Part2_Repository.Data;

public class BookshopContext : DbContext
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=bookshop.db");
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Author>().HasKey(a => a.Id);
        mb.Entity<Book>().HasKey(b => b.Id);

        mb.Entity<Book>()
            .HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId);
    }
}
