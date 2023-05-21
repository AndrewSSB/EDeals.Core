using EDeals.Core.Domain.Enums;
using EDeals.Core.Infrastructure.Identity.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EDeals.Core.Infrastructure.Seeders
{
    public class IdentityRoleSeeder
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public IdentityRoleSeeder(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task CreateRoles()
        {
            var applicationRoles = await _roleManager.Roles.Select(x => x.Name).ToListAsync();

            var roleNames = new List<string>
            {
                RoleNames.UserRole,
                RoleNames.AdminRole,
                RoleNames.SuperAdminRole
            };

            var roles = roleNames.Except(applicationRoles);

            if (!roles.Any())
            {
                return;
            }

            foreach (var role in roles)
            {
                await _roleManager.CreateAsync(new ApplicationRole { Name = role });
            }
        }
    }
}
