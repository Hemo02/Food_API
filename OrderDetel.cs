namespace FoodApi.Models
{
    public class OrderDetel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Products productss { get; set; }
        public int OrderTd { get; set; }
        public Order order { get; set; }
        public double OrderTotel { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

             
    }
}
