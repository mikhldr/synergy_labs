using Microsoft.EntityFrameworkCore;
using Part2_Repository.Data;
using Part2_Repository.Entities;
using Part2_Repository.Interfaces;

namespace Part2_Repository.Repositories;

public class BookRepository : GenericRepository<Book>, IBookRepository
{
    public BookRepository(BookshopContext ctx) : base(ctx) { }

    public IEnumerable<Book> GetByAuthor(int authorId)
        => _set.Include(b => b.Author)
               .Where(b => b.AuthorId == authorId)
               .ToList();

    public IEnumerable<Book> GetByPriceRange(decimal min, decimal max)
        => _set.Include(b => b.Author)
               .Where(b => b.Price >= min && b.Price <= max)
               .ToList();

    public IEnumerable<Book> SearchByTitle(string keyword)
        => _set.Include(b => b.Author)
               .Where(b => b.Title.Contains(keyword))
               .ToList();
}
