using System.Linq.Expressions;

namespace AdvancedGSTApp.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    IQueryable<T> Query();
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    void Update(T entity);
    void SoftDelete(T entity);
    Task<int> SaveChangesAsync();
}
