namespace FoodApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Adrees { get; set; }
        public string Phone { get; set; }
        public double Totel { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsComplet { get; set; }
        public int UserId { get; set; }
        public ICollection<OrderDetel> orderDetels { get; set; }

        
    }
}
