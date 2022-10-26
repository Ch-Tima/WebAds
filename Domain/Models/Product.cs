﻿
namespace Domain.Models
{
    public class Ads
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Content { get; set; }

        public decimal Price { get; set; }

        public DateTime DateCreate { get; set; }

        public bool IsVerified { get; set; }
        public bool IsTop { get; set; }


        public int CityId { get; set; }
        public City City { get; set; }

        public int CategotyId { get; set; }
        public Category Categoty { get; set; }


        public int UserId { get; set; }
        public User User { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
