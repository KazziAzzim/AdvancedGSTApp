using AdvancedGSTApp.Data;
using AdvancedGSTApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace AdvancedGSTApp.Services.SaaS;

public class ApplicationClaimsPrincipalFactory(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IOptions<IdentityOptions> optionsAccessor,
    ApplicationDbContext db) : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>(userManager, roleManager, optionsAccessor)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        identity.AddClaim(new Claim("is_platform_user", user.IsPlatformUser.ToString().ToLowerInvariant()));

        if (user.TenantId.HasValue)
        {
            identity.AddClaim(new Claim("tenant_id", user.TenantId.Value.ToString()));
            var tenant = await db.Tenants.FindAsync(user.TenantId.Value);
            if (tenant is not null)
            {
                identity.AddClaim(new Claim("tenant_name", tenant.BusinessName));
            }
        }

        return identity;
    }
}
