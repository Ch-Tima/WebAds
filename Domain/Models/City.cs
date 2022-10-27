using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public List<Ad> Ads { get; set; }
    }
}
