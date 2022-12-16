namespace Domain.Models
{
    public class Category
    {
        public Category()
        {
            Ads = new List<Ad>();
            Categories = new List<Category>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        
        public int? CategoryId { get; set; }
        public Category? Categors { get; set; }

        public virtual ICollection<Ad> Ads { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
