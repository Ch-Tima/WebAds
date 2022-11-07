namespace Domain.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string PathImg { get; set; }

        public int AdId { get; set; }
        public Ad Ad { get; set; }
    }
}
