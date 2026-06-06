using AdvancedGSTApp.Models;

namespace AdvancedGSTApp.Services.Interfaces;
public interface IServiceItemService
{
    Task<List<ServiceItem>> GetAllAsync();
    Task<ServiceItem?> GetByIdAsync(int id);
    Task SaveAsync(ServiceItem entity);
    Task DeleteAsync(int id);
}
