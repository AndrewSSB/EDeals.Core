using EDeals.Core.Infrastructure.Identity.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EDeals.Core.Infrastructure.EntityConfiguration
{
    internal class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasMany(ur => ur.UserRoles)
                .WithOne(r => r.Role)
                .HasForeignKey(r => r.RoleId)
                .IsRequired();
        }
    }
}
