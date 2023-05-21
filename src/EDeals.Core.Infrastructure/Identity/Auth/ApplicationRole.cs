using Microsoft.AspNetCore.Identity;

namespace EDeals.Core.Infrastructure.Identity.Auth
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
