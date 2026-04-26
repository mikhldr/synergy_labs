using Repository.Entities;

namespace Repository.Interfaces;

public interface IBookRepository : IRepository<Book>
{
    IEnumerable<Book> GetByAuthor(int authorId);
    IEnumerable<Book> GetByPriceRange(decimal min, decimal max);
    IEnumerable<Book> SearchByTitle(string keyword);
}
