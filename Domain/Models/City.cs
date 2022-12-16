namespace Domain.Models
{
    public class City
    {
        public City()
        {
            Ads = new List<Ad>();
            Users = new List<User>();
        }


        /// <summary>
        /// Name - this is identifier "ID"
        /// </summary>
        public string Name { get; set; }
        public string Region { get; set; }
        public virtual ICollection<Ad> Ads { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
