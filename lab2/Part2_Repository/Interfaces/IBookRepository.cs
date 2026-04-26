using Part2_Repository.Entities;

namespace Part2_Repository.Interfaces;

public interface IBookRepository : IRepository<Book>
{
    IEnumerable<Book> GetByAuthor(int authorId);
    IEnumerable<Book> GetByPriceRange(decimal min, decimal max);
    IEnumerable<Book> SearchByTitle(string keyword);
}
