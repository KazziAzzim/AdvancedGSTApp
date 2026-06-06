using AdvancedGSTApp.Models;

namespace AdvancedGSTApp.Services.Interfaces;
public interface ICustomerService
{
    Task<List<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(int id);
    Task SaveAsync(Customer entity);
    Task DeleteAsync(int id);
}
