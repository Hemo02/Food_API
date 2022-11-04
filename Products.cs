using System.ComponentModel.DataAnnotations.Schema;

namespace FoodApi.Models
{
    public class Products
    {
        public int id { get; set; }
        public string Titel { get; set; }
        public string Discription { get; set; }
        public string imageurl { get; set; }
        public double price { get; set; }
        public bool Ispopular { get; set; }
        public int CategoryId { get; set; }
        [NotMapped]
        public byte[] ImageArray { get; set; }
        public ICollection<OrderDetel> GetOrderDetels { get; set; }
        public ICollection<Cart> carts { get; set; }
    }
}
