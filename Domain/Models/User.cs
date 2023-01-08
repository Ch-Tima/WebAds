using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            Comments = new List<Comment>();
            Ads = new List<Ad>();
            FavoritesAds = new List<FavouritesAd>();
        }

        public string Surname { get; set; }
        public string IconPath { get; set; }
        
        public string? CityName { get; set; }
        public City City { get; set; }


        public bool IsMailing { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Ad> Ads { get; set; }
        public virtual ICollection<FavouritesAd> FavoritesAds { get; set; }
    }
}
