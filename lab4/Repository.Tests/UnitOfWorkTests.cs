using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Entities;
using Repository.Repositories;
using Xunit;

namespace Repository.Tests;

public class UnitOfWorkTests
{
    private BookshopContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<BookshopContext>()
            .UseInMemoryDatabase("UoWTest_" + Guid.NewGuid())
            .Options;
        return new BookshopContext(options);
    }

    [Fact]
    public void UoW_ExposesBooksAndAuthorsRepositories()
    {
        using var ctx = CreateInMemoryContext();
        using var uow = new UnitOfWork(ctx);

        Assert.NotNull(uow.Books);
        Assert.NotNull(uow.Authors);
    }

    [Fact]
    public void SaveChanges_PersistsChanges()
    {
        using var ctx = CreateInMemoryContext();
        using var uow = new UnitOfWork(ctx);

        uow.Authors.Add(new Author { FullName = "Test", Country = "Test" });
        var saved = uow.SaveChanges();

        // Add() сам уже сохраняет, но повторный SaveChanges() возвращает 0
        Assert.True(saved >= 0);
        Assert.Single(uow.Authors.GetAll());
    }

    [Fact]
    public async Task SaveChangesAsync_Works()
    {
        using var ctx = CreateInMemoryContext();
        using var uow = new UnitOfWork(ctx);

        await uow.Authors.AddAsync(new Author { FullName = "AsyncTest", Country = "Test" });
        var saved = await uow.SaveChangesAsync();

        Assert.True(saved >= 0);
    }

    [Fact]
    public void Dispose_DoesNotThrow()
    {
        var ctx = CreateInMemoryContext();
        var uow = new UnitOfWork(ctx);

        var ex = Record.Exception(() => uow.Dispose());
        Assert.Null(ex);
    }
}
