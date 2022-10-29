using Microsoft.AspNetCore.Identity;


namespace Domain.Models
{
    public class User : IdentityUser
    {
        public string Surname { get; set; }
        public string IconPath { get; set; }
        public string Address { get; set; }


        public bool IsMailing { get; set; }

        public List<Comment> Comments { get; set; }
        public List<Ad> Ads { get; set; }
    }
}
