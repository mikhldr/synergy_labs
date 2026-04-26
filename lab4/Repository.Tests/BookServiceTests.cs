using Moq;
using Repository.Entities;
using Repository.Interfaces;
using Repository.Services;
using Xunit;

namespace Repository.Tests;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _repoMock;
    private readonly BookService _service;

    public BookServiceTests()
    {
        _repoMock = new Mock<IBookRepository>();
        _service = new BookService(_repoMock.Object);
    }

    // ====== Конструктор ======
    [Fact]
    public void Ctor_ThrowsArgumentNullException_WhenRepoIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new BookService(null!));
    }

    // ====== CalculateOrderCost ======
    [Fact]
    public void CalculateOrderCost_ThrowsArgumentNullException_WhenBookIdsIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _service.CalculateOrderCost(null!));
    }

    [Fact]
    public void CalculateOrderCost_ThrowsArgumentException_WhenListIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => _service.CalculateOrderCost(new List<int>()));
    }

    [Fact]
    public void CalculateOrderCost_ThrowsInvalidOperation_WhenBookNotFound()
    {
        _repoMock.Setup(r => r.GetById(99)).Returns((Book?)null);

        Assert.Throws<InvalidOperationException>(
            () => _service.CalculateOrderCost(new[] { 99 }));
    }

    [Fact]
    public void CalculateOrderCost_ReturnsSum_WhenBelowDiscountThreshold()
    {
        _repoMock.Setup(r => r.GetById(1)).Returns(new Book { Id = 1, Price = 100m });
        _repoMock.Setup(r => r.GetById(2)).Returns(new Book { Id = 2, Price = 200m });

        var result = _service.CalculateOrderCost(new[] { 1, 2 });

        Assert.Equal(300m, result);
    }

    [Fact]
    public void CalculateOrderCost_AppliesDiscount_WhenAtOrAboveThreshold()
    {
        _repoMock.Setup(r => r.GetById(1)).Returns(new Book { Id = 1, Price = 300m });
        _repoMock.Setup(r => r.GetById(2)).Returns(new Book { Id = 2, Price = 300m });

        var result = _service.CalculateOrderCost(new[] { 1, 2 });

        // 600 - 10% = 540
        Assert.Equal(540m, result);
    }

    // ====== CategorizeByPrice ======
    [Fact]
    public void CategorizeByPrice_ThrowsArgumentNullException_WhenBookIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _service.CategorizeByPrice(null!));
    }

    [Theory]
    [InlineData(50, "Бюджет")]
    [InlineData(99.99, "Бюджет")]
    [InlineData(100, "Стандарт")]
    [InlineData(499.99, "Стандарт")]
    [InlineData(500, "Премиум")]
    [InlineData(999.99, "Премиум")]
    [InlineData(1000, "Элитная")]
    [InlineData(5000, "Элитная")]
    public void CategorizeByPrice_ReturnsCorrectCategory(decimal price, string expected)
    {
        var book = new Book { Price = price };

        var result = _service.CategorizeByPrice(book);

        Assert.Equal(expected, result);
    }

    // ====== FindCheapestByAuthor ======
    [Fact]
    public void FindCheapestByAuthor_ThrowsArgumentException_WhenIdIsZeroOrNegative()
    {
        Assert.Throws<ArgumentException>(() => _service.FindCheapestByAuthor(0));
        Assert.Throws<ArgumentException>(() => _service.FindCheapestByAuthor(-5));
    }

    [Fact]
    public void FindCheapestByAuthor_ReturnsNull_WhenNoBooks()
    {
        _repoMock.Setup(r => r.GetByAuthor(1)).Returns(new List<Book>());

        var result = _service.FindCheapestByAuthor(1);

        Assert.Null(result);
    }

    [Fact]
    public void FindCheapestByAuthor_ReturnsCheapest_WhenBooksExist()
    {
        _repoMock.Setup(r => r.GetByAuthor(1)).Returns(new List<Book>
        {
            new Book { Id = 1, Title = "Дорогая", Price = 800m },
            new Book { Id = 2, Title = "Дешёвая", Price = 200m },
            new Book { Id = 3, Title = "Средняя", Price = 500m },
        });

        var result = _service.FindCheapestByAuthor(1);

        Assert.NotNull(result);
        Assert.Equal("Дешёвая", result!.Title);
    }
}
