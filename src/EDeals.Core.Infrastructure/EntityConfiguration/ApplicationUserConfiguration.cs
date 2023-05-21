using EDeals.Core.Infrastructure.Identity.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EDeals.Core.Infrastructure.EntityConfiguration
{
    internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.FirstName)
                .HasMaxLength(50);
            
            builder.Property(x => x.LastName)
                .HasMaxLength(50);

            builder.Property(x => x.UserName)
                .HasMaxLength(50);

            builder.Property(x => x.NormalizedUserName)
                .HasMaxLength(50);

            builder.Property(x => x.Email)
                .HasMaxLength(80);
            
            builder.Property(x => x.NormalizedEmail)
                .HasMaxLength(80);

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(15);

            builder.HasMany(x => x.UserRoles)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .IsRequired();
        }
    }
}
