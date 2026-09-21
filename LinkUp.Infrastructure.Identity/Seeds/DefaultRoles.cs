using LinkUp.Core.Domain.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace LinkUp.Infrastructure.Identity.Seeds
{
    public class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            
        }
    }
}
