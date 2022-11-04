namespace FoodApi.Models
{
    public class Cart
    {
        public int id { get; set; }
        public int ProdectId { get; set; }
        public double price { get; set; }
        public int Quantity { get; set; }
        public double TotelAmount { get; set; }
        public int CustomeruId { get; set; }
    }
}
