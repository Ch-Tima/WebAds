namespace Domain.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; }
        public DateTime DateCreate { get; set; }


        public int UserId { get; set; }
        public User User { get; set; }


        public int AdsId { get; set; }
        public Ads Ads { get; set; }

    }
}