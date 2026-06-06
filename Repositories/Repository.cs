using AdvancedGSTApp.Data;
using AdvancedGSTApp.Models;
using AdvancedGSTApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AdvancedGSTApp.Repositories;

public class Repository<T>(ApplicationDbContext db) : IRepository<T> where T : class
{
    public IQueryable<T> Query() => db.Set<T>().AsQueryable();
    public Task<List<T>> GetAllAsync() => db.Set<T>().ToListAsync();
    public async Task<T?> GetByIdAsync(int id) => await db.Set<T>().FindAsync(id);
    public async Task AddAsync(T entity) => await db.Set<T>().AddAsync(entity);
    public void Update(T entity) => db.Set<T>().Update(entity);
    public void SoftDelete(T entity)
    {
        if (entity is AuditableEntity auditable) { auditable.IsDeleted = true; auditable.DeletedDate = DateTime.UtcNow; db.Set<T>().Update(entity); }
        else db.Set<T>().Remove(entity);
    }
    public Task<int> SaveChangesAsync() => db.SaveChangesAsync();
}
