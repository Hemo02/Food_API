using Microsoft.EntityFrameworkCore;

namespace FoodApi.Models
{
    public class RestDbContaxt :DbContext
    {
        public RestDbContaxt(DbContextOptions<RestDbContaxt> options) :base(options)
        {

        }

        
        public DbSet<User> users { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderDetel> orderDetels { get; set; }
        public DbSet<Products> productss { get; set; }

    }
}
