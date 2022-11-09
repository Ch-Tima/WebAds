using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class City
    {
        /// <summary>
        /// Name - this is identifier "ID"
        /// </summary>
        public string Name { get; set; }
        public string Region { get; set; }
        public List<Ad> Ads { get; set; }
        public List<User> Users { get; set; }
    }
}
