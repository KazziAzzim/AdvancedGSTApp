using AdvancedGSTApp.Models;
using AdvancedGSTApp.Repositories.Interfaces;
using AdvancedGSTApp.Services.Interfaces;

namespace AdvancedGSTApp.Services;
public class CustomerService(IRepository<Customer> repository) : ICustomerService
{
    public Task<List<Customer>> GetAllAsync() => repository.GetAllAsync();
    public Task<Customer?> GetByIdAsync(int id) => repository.GetByIdAsync(id);
    public async Task SaveAsync(Customer entity) { if (entity.Id == 0) await repository.AddAsync(entity); else repository.Update(entity); await repository.SaveChangesAsync(); }
    public async Task DeleteAsync(int id) { var entity = await repository.GetByIdAsync(id); if (entity is not null) { repository.SoftDelete(entity); await repository.SaveChangesAsync(); } }
}
