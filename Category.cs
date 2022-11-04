using System.ComponentModel.DataAnnotations.Schema;

namespace FoodApi.Models
{
    public class Category
    {
        public int id { get;set;}
        public string Titel { get; set;}
        public string iamgeurl { get; set;}
        [NotMapped]
        public byte[] ImageArray { get; set; }
        public ICollection<Products> productss { get; set; }
    }
}
