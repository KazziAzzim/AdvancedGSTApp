using AdvancedGSTApp.Models;
using AdvancedGSTApp.Repositories.Interfaces;
using AdvancedGSTApp.Services.Interfaces;

namespace AdvancedGSTApp.Services;
public class SupplierService(IRepository<Supplier> repository) : ISupplierService
{
    public Task<List<Supplier>> GetAllAsync() => repository.GetAllAsync();
    public Task<Supplier?> GetByIdAsync(int id) => repository.GetByIdAsync(id);
    public async Task SaveAsync(Supplier entity) { if (entity.Id == 0) await repository.AddAsync(entity); else repository.Update(entity); await repository.SaveChangesAsync(); }
    public async Task DeleteAsync(int id) { var entity = await repository.GetByIdAsync(id); if (entity is not null) { repository.SoftDelete(entity); await repository.SaveChangesAsync(); } }
}
