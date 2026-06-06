using AdvancedGSTApp.Models;

namespace AdvancedGSTApp.Services.Interfaces;
public interface ISupplierService
{
    Task<List<Supplier>> GetAllAsync();
    Task<Supplier?> GetByIdAsync(int id);
    Task SaveAsync(Supplier entity);
    Task DeleteAsync(int id);
}
