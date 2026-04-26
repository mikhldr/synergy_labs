using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Services;

public class BookService
{
    private readonly IBookRepository _repo;
    private const decimal DiscountThreshold = 500m;
    private const decimal DiscountPercent = 0.10m;

    public BookService(IBookRepository repo)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
    }

    public decimal CalculateOrderCost(IEnumerable<int> bookIds)
    {
        if (bookIds == null)
            throw new ArgumentNullException(nameof(bookIds));

        decimal total = 0;
        int count = 0;

        foreach (var id in bookIds)
        {
            var book = _repo.GetById(id);
            if (book == null)
                throw new InvalidOperationException($"Книга с id={id} не найдена");

            total += book.Price;
            count++;
        }

        if (count == 0)
            throw new ArgumentException("Список книг не может быть пустым", nameof(bookIds));

        if (total >= DiscountThreshold)
        {
            total -= total * DiscountPercent;
        }

        return total;
    }

    public string CategorizeByPrice(Book book)
    {
        if (book == null)
            throw new ArgumentNullException(nameof(book));

        return book.Price switch
        {
            < 100 => "Бюджет",
            >= 100 and < 500 => "Стандарт",
            >= 500 and < 1000 => "Премиум",
            _ => "Элитная"
        };
    }

    public Book? FindCheapestByAuthor(int authorId)
    {
        if (authorId <= 0)
            throw new ArgumentException("authorId должен быть положительным", nameof(authorId));

        var books = _repo.GetByAuthor(authorId).ToList();
        if (books.Count == 0)
            return null;

        return books.OrderBy(b => b.Price).First();
    }
}
