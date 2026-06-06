using AdvancedGSTApp.Models;

namespace AdvancedGSTApp.Services.Interfaces;
public interface IProductService
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task SaveAsync(Product entity);
    Task DeleteAsync(int id);
}
