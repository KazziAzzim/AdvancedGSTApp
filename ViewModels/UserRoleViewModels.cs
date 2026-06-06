using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AdvancedGSTApp.Models;

public class UserListItemViewModel
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class UserListViewModel
{
    public List<UserListItemViewModel> Users { get; set; } = [];
    public string? Search { get; set; }
    public string? RoleId { get; set; }
    public bool? IsActive { get; set; }
    public int Page { get; set; } = 1;
    public int TotalPages { get; set; }
    public List<SelectListItem> Roles { get; set; } = [];
}

public class CreateUserViewModel
{
    [Required, StringLength(150)] public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Phone] public string? PhoneNumber { get; set; }
    [Required, StringLength(100)] public string UserName { get; set; } = string.Empty;
    [Required, DataType(DataType.Password), StringLength(100, MinimumLength = 8)] public string Password { get; set; } = string.Empty;
    [Required, DataType(DataType.Password), Compare(nameof(Password))] public string ConfirmPassword { get; set; } = string.Empty;
    [Required] public string RoleId { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string? ProfilePhoto { get; set; }
    public List<SelectListItem> Roles { get; set; } = [];
}

public class EditUserViewModel
{
    public string Id { get; set; } = string.Empty;
    [Required, StringLength(150)] public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Phone] public string? PhoneNumber { get; set; }
    [Required, StringLength(100)] public string UserName { get; set; } = string.Empty;
    [Required] public string RoleId { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? ProfilePhoto { get; set; }
    public List<SelectListItem> Roles { get; set; } = [];
}

public class UserDetailsViewModel : EditUserViewModel
{
    public string RoleName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
}

public class ChangeUserPasswordViewModel
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    [Required, DataType(DataType.Password), StringLength(100, MinimumLength = 8)] public string Password { get; set; } = string.Empty;
    [Required, DataType(DataType.Password), Compare(nameof(Password))] public string ConfirmPassword { get; set; } = string.Empty;
}

public class RoleListItemViewModel
{
    public string Id { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int UserCount { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class RoleListViewModel
{
    public List<RoleListItemViewModel> Roles { get; set; } = [];
    public string? Search { get; set; }
}

public class CreateRoleViewModel
{
    [Required, StringLength(100)] public string RoleName { get; set; } = string.Empty;
    [StringLength(500)] public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}

public class EditRoleViewModel : CreateRoleViewModel
{
    public string Id { get; set; } = string.Empty;
}

public class RoleDetailsViewModel : EditRoleViewModel
{
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int UserCount { get; set; }
}

public class RolePermissionViewModel
{
    public int Id { get; set; }
    public string ModuleName { get; set; } = string.Empty;
    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanPrint { get; set; }
    public bool CanExport { get; set; }
    public bool CanEmail { get; set; }
    public bool CanGenerate { get; set; }
    public bool CanApprove { get; set; }
}

public class ManageRolePermissionsViewModel
{
    public string RoleId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public List<RolePermissionViewModel> Permissions { get; set; } = [];
}
