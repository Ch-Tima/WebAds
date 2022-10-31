
namespace Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public int? CategoryId { get; set; }
        public Category? Categors { get; set; }

        public List<Ad> Ads { get; set; }
        public List<Category> Categories { get; set; }
    }
}
