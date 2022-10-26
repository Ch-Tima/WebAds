using Microsoft.AspNetCore.Identity;


namespace Domain.Models
{
    public class User : IdentityUser
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string IconPath { get; set; }
        public string Address { get; set; }
        public string NumberPhone { get; set; }

        public List<Comment> Comments { get; set; }//???
        public List<Ads> Adss { get; set; }
    }
}
