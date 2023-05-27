using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDeals.Core.Infrastructure.Identity.Auth
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser() { }

        public ApplicationUser(string firstName,
            string lastName,
            string? userName,
            string? email,
            string? phoneNumber)
        { 
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;  
            Email = email;
            PhoneNumber = phoneNumber;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } 
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; } 
        
        public bool IsDeleted { get; set; }
        
        public string FirstName { get; private set; }
        
        public string LastName { get; private set; }

        public string DigitCode { get; set; }

        public DateTime? ResendCodeAvailableAfter { get; set; }
        public DateTime? ResendTokenAvailableAfter { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        // Navigation properties
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
