using Microsoft.EntityFrameworkCore;
using Repository.Entities;

namespace Repository.Data;

public class BookshopContext : DbContext
{
    public BookshopContext() { }

    public BookshopContext(DbContextOptions<BookshopContext> options) : base(options) { }

    public virtual DbSet<Book> Books => Set<Book>();
    public virtual DbSet<Author> Authors => Set<Author>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            options.UseSqlite("Data Source=bookshop.db");
        }
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
