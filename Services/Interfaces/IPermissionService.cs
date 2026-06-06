namespace AdvancedGSTApp.Services.Interfaces;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync(string userId, string moduleName, string permissionName);
    Task<bool> CanViewAsync(string userId, string moduleName);
    Task<bool> CanCreateAsync(string userId, string moduleName);
    Task<bool> CanEditAsync(string userId, string moduleName);
    Task<bool> CanDeleteAsync(string userId, string moduleName);
    Task<bool> CanPrintAsync(string userId, string moduleName);
    Task<bool> CanExportAsync(string userId, string moduleName);
    Task<bool> CanEmailAsync(string userId, string moduleName);
    Task<bool> CanGenerateAsync(string userId, string moduleName);
    Task<bool> CanApproveAsync(string userId, string moduleName);
}
