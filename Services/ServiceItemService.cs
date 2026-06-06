using AdvancedGSTApp.Models;
using AdvancedGSTApp.Repositories.Interfaces;
using AdvancedGSTApp.Services.Interfaces;

namespace AdvancedGSTApp.Services;
public class ServiceItemService(IRepository<ServiceItem> repository) : IServiceItemService
{
    public Task<List<ServiceItem>> GetAllAsync() => repository.GetAllAsync();
    public Task<ServiceItem?> GetByIdAsync(int id) => repository.GetByIdAsync(id);
    public async Task SaveAsync(ServiceItem entity) { if (entity.Id == 0) await repository.AddAsync(entity); else repository.Update(entity); await repository.SaveChangesAsync(); }
    public async Task DeleteAsync(int id) { var entity = await repository.GetByIdAsync(id); if (entity is not null) { repository.SoftDelete(entity); await repository.SaveChangesAsync(); } }
}
