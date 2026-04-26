using Microsoft.EntityFrameworkCore;
using Part2_Repository.Data;
using Part2_Repository.Interfaces;

namespace Part2_Repository.Repositories;

// Универсальный репозиторий — повышенный уровень
public class GenericRepository<T> : IRepository<T> where T : class
{
    protected readonly BookshopContext _ctx;
    protected readonly DbSet<T> _set;

    public GenericRepository(BookshopContext ctx)
    {
        _ctx = ctx;
        _set = ctx.Set<T>();
    }

    public T? GetById(int id) => _set.Find(id);
    public IEnumerable<T> GetAll() => _set.ToList();

    public void Add(T entity)
    {
        _set.Add(entity);
        _ctx.SaveChanges();
    }

    public void Update(T entity)
    {
        _set.Update(entity);
        _ctx.SaveChanges();
    }

    public void Delete(int id)
    {
        var entity = _set.Find(id);
        if (entity != null)
        {
            _set.Remove(entity);
            _ctx.SaveChanges();
        }
    }

    // Асинхронные версии
    public async Task<T?> GetByIdAsync(int id) => await _set.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync() => await _set.ToListAsync();

    public async Task AddAsync(T entity)
    {
        await _set.AddAsync(entity);
        await _ctx.SaveChangesAsync();
    }
}
