using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Interfaces;

namespace Repository.Repositories;

public class GenericRepository<T> : IRepository<T> where T : class
{
    protected readonly BookshopContext _ctx;
    protected readonly DbSet<T> _set;

    public GenericRepository(BookshopContext ctx)
    {
        _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        _set = ctx.Set<T>();
    }

    public T? GetById(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Id должен быть положительным", nameof(id));
        return _set.Find(id);
    }

    public IEnumerable<T> GetAll() => _set.ToList();

    public void Add(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _set.Add(entity);
        _ctx.SaveChanges();
    }

    public void Update(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _set.Update(entity);
        _ctx.SaveChanges();
    }

    public void Delete(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Id должен быть положительным", nameof(id));

        var entity = _set.Find(id);
        if (entity == null)
            throw new InvalidOperationException($"Сущность с id={id} не найдена");

        _set.Remove(entity);
        _ctx.SaveChanges();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Id должен быть положительным", nameof(id));
        return await _set.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _set.ToListAsync();

    public async Task AddAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await _set.AddAsync(entity);
        await _ctx.SaveChangesAsync();
    }
}
