using Microsoft.AspNetCore.Identity;
namespace Domain.Models
{
    public class UserRole : IdentityUser<string>
    {
        public string RoleId { get; set; }
        public Role Role { get; set; }


        public string UserId { get; set; }
        public User User { get; set; }
    }
}
