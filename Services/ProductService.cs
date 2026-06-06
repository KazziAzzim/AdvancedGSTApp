using AdvancedGSTApp.Models;
using AdvancedGSTApp.Repositories.Interfaces;
using AdvancedGSTApp.Services.Interfaces;

namespace AdvancedGSTApp.Services;
public class ProductService(IRepository<Product> repository) : IProductService
{
    public Task<List<Product>> GetAllAsync() => repository.GetAllAsync();
    public Task<Product?> GetByIdAsync(int id) => repository.GetByIdAsync(id);
    public async Task SaveAsync(Product entity) { if (entity.Id == 0) await repository.AddAsync(entity); else repository.Update(entity); await repository.SaveChangesAsync(); }
    public async Task DeleteAsync(int id) { var entity = await repository.GetByIdAsync(id); if (entity is not null) { repository.SoftDelete(entity); await repository.SaveChangesAsync(); } }
}
