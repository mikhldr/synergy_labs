using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Entities;
using Repository.Repositories;
using Xunit;

namespace Repository.Tests;

public class BookRepositoryTests : IDisposable
{
    private readonly BookshopContext _ctx;
    private readonly BookRepository _repo;

    public BookRepositoryTests()
    {
        // Уникальное имя БД на каждый тест-класс, чтобы тесты не пересекались
        var options = new DbContextOptionsBuilder<BookshopContext>()
            .UseInMemoryDatabase("TestDb_" + Guid.NewGuid())
            .Options;
        _ctx = new BookshopContext(options);
        _repo = new BookRepository(_ctx);

        SeedData();
    }

    private void SeedData()
    {
        var pushkin = new Author { Id = 1, FullName = "Пушкин", Country = "Россия" };
        var tolkien = new Author { Id = 2, FullName = "Толкин", Country = "Великобритания" };
        _ctx.Authors.AddRange(pushkin, tolkien);

        _ctx.Books.AddRange(
            new Book { Id = 1, Title = "Евгений Онегин", Price = 250m, Year = 1833, AuthorId = 1 },
            new Book { Id = 2, Title = "Капитанская дочка", Price = 180m, Year = 1836, AuthorId = 1 },
            new Book { Id = 3, Title = "Властелин колец", Price = 950m, Year = 1954, AuthorId = 2 },
            new Book { Id = 4, Title = "Хоббит", Price = 420m, Year = 1937, AuthorId = 2 }
        );
        _ctx.SaveChanges();
    }

    public void Dispose() => _ctx.Dispose();

    // ====== Generic CRUD ======
    [Fact]
    public void Ctor_ThrowsArgumentNullException_WhenContextIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new BookRepository(null!));
    }

    [Fact]
    public void GetById_ThrowsArgumentException_WhenIdIsZeroOrNegative()
    {
        Assert.Throws<ArgumentException>(() => _repo.GetById(0));
        Assert.Throws<ArgumentException>(() => _repo.GetById(-1));
    }

    [Fact]
    public void GetById_ReturnsBook_WhenExists()
    {
        var book = _repo.GetById(1);
        Assert.NotNull(book);
        Assert.Equal("Евгений Онегин", book!.Title);
    }

    [Fact]
    public void GetById_ReturnsNull_WhenNotFound()
    {
        var book = _repo.GetById(999);
        Assert.Null(book);
    }

    [Fact]
    public void GetAll_ReturnsAllBooks()
    {
        var books = _repo.GetAll();
        Assert.Equal(4, books.Count());
    }

    [Fact]
    public void Add_ThrowsArgumentNullException_WhenEntityIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _repo.Add(null!));
    }

    [Fact]
    public void Add_AddsBook_WhenValid()
    {
        var newBook = new Book { Title = "Новая книга", Price = 100m, Year = 2024, AuthorId = 1 };
        _repo.Add(newBook);

        Assert.Equal(5, _repo.GetAll().Count());
    }

    [Fact]
    public void Update_ThrowsArgumentNullException_WhenEntityIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _repo.Update(null!));
    }

    [Fact]
    public void Update_ChangesBookFields()
    {
        var book = _repo.GetById(1)!;
        book.Price = 999m;
        _repo.Update(book);

        var updated = _repo.GetById(1);
        Assert.Equal(999m, updated!.Price);
    }

    [Fact]
    public void Delete_ThrowsArgumentException_WhenIdInvalid()
    {
        Assert.Throws<ArgumentException>(() => _repo.Delete(0));
    }

    [Fact]
    public void Delete_ThrowsInvalidOperation_WhenBookNotFound()
    {
        Assert.Throws<InvalidOperationException>(() => _repo.Delete(999));
    }

    [Fact]
    public void Delete_RemovesBook_WhenExists()
    {
        _repo.Delete(1);
        Assert.Null(_repo.GetById(1));
        Assert.Equal(3, _repo.GetAll().Count());
    }

    // ====== Async ======
    [Fact]
    public async Task GetByIdAsync_ThrowsArgumentException_WhenIdInvalid()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _repo.GetByIdAsync(0));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsBook()
    {
        var b = await _repo.GetByIdAsync(2);
        Assert.NotNull(b);
        Assert.Equal("Капитанская дочка", b!.Title);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAll()
    {
        var all = await _repo.GetAllAsync();
        Assert.Equal(4, all.Count());
    }

    [Fact]
    public async Task AddAsync_ThrowsArgumentNullException_WhenEntityIsNull()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _repo.AddAsync(null!));
    }

    [Fact]
    public async Task AddAsync_AddsBook()
    {
        await _repo.AddAsync(new Book { Title = "Async-книга", Price = 50m, Year = 2024, AuthorId = 1 });
        var all = await _repo.GetAllAsync();
        Assert.Equal(5, all.Count());
    }

    // ====== Specialized methods ======
    [Fact]
    public void GetByAuthor_ThrowsArgumentException_WhenIdInvalid()
    {
        Assert.Throws<ArgumentException>(() => _repo.GetByAuthor(0));
    }

    [Fact]
    public void GetByAuthor_ReturnsCorrectBooks()
    {
        var books = _repo.GetByAuthor(1).ToList();
        Assert.Equal(2, books.Count);
        Assert.All(books, b => Assert.Equal(1, b.AuthorId));
    }

    [Fact]
    public void GetByPriceRange_ThrowsArgumentException_WhenMinIsNegative()
    {
        Assert.Throws<ArgumentException>(() => _repo.GetByPriceRange(-1m, 100m));
    }

    [Fact]
    public void GetByPriceRange_ThrowsArgumentException_WhenMaxLessThanMin()
    {
        Assert.Throws<ArgumentException>(() => _repo.GetByPriceRange(500m, 100m));
    }

    [Fact]
    public void GetByPriceRange_ReturnsBooksInRange()
    {
        var books = _repo.GetByPriceRange(200m, 500m).ToList();
        Assert.Equal(2, books.Count);
        Assert.All(books, b => Assert.InRange(b.Price, 200m, 500m));
    }

    [Fact]
    public void SearchByTitle_ThrowsArgumentException_WhenKeywordIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => _repo.SearchByTitle(""));
        Assert.Throws<ArgumentException>(() => _repo.SearchByTitle("   "));
        Assert.Throws<ArgumentException>(() => _repo.SearchByTitle(null!));
    }

    [Fact]
    public void SearchByTitle_ReturnsMatching()
    {
        var found = _repo.SearchByTitle("кол").ToList();
        Assert.Single(found);
        Assert.Equal("Властелин колец", found[0].Title);
    }
}
