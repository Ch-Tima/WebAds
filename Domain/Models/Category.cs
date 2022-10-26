
namespace Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public int CategotyId { get; set; }


        public List<Ads> Adss { get; set; }
        public List<Category> Categoties { get; set; }
    }
}
