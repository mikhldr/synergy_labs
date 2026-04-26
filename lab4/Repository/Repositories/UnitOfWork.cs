using Repository.Data;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories;

// Координирует работу нескольких репозиториев
public class UnitOfWork : IDisposable
{
    private readonly BookshopContext _ctx;

    public IBookRepository Books { get; }
    public IRepository<Author> Authors { get; }

    public UnitOfWork(BookshopContext ctx)
    {
        _ctx = ctx;
        Books = new BookRepository(ctx);
        Authors = new GenericRepository<Author>(ctx);
    }

    public int SaveChanges() => _ctx.SaveChanges();
    public Task<int> SaveChangesAsync() => _ctx.SaveChangesAsync();

    public void Dispose() => _ctx.Dispose();
}
