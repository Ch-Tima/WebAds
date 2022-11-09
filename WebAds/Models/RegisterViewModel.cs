namespace WebAds.Models
{
    public class RegisterViewModel
    {

        public string Name { get; set; }
        public string Surname { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        

        public string CityName { get; set; }

        public bool IsMailing { get; set; }
        
    }
}