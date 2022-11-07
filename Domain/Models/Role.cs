using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class Role : IdentityRole
    {
        public Role(string roleName)
        {
            this.Name = roleName;
        }
        public List<UserRole> UserRoles { get; set; }
    }
}
