using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories;

public class BookRepository : GenericRepository<Book>, IBookRepository
{
    public BookRepository(BookshopContext ctx) : base(ctx) { }

    public IEnumerable<Book> GetByAuthor(int authorId)
    {
        if (authorId <= 0)
            throw new ArgumentException("authorId должен быть положительным", nameof(authorId));

        return _set.Include(b => b.Author)
                   .Where(b => b.AuthorId == authorId)
                   .ToList();
    }

    public IEnumerable<Book> GetByPriceRange(decimal min, decimal max)
    {
        if (min < 0)
            throw new ArgumentException("Минимальная цена не может быть отрицательной", nameof(min));
        if (max < min)
            throw new ArgumentException("Максимальная цена не может быть меньше минимальной", nameof(max));

        return _set.Include(b => b.Author)
                   .Where(b => b.Price >= min && b.Price <= max)
                   .ToList();
    }

    public IEnumerable<Book> SearchByTitle(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            throw new ArgumentException("Ключевое слово не может быть пустым", nameof(keyword));

        return _set.Include(b => b.Author)
                   .Where(b => b.Title.Contains(keyword))
                   .ToList();
    }
}
