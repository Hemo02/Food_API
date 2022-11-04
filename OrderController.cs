using FoodApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Runtime.CompilerServices;

namespace FoodApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly RestDbContaxt _OrderDbContaxt;

        public OrderController(RestDbContaxt OrderDbContaxt)
        {
            _OrderDbContaxt = OrderDbContaxt;
        }


        //::::::::::::::::::: S :::::::::::::::::::
        [HttpPost]
        public IActionResult post([FromBody]Order order)
        {
            order.IsComplet = false;
            order.DateTime = DateTime.Now;
            _OrderDbContaxt.orders.Add(order);
            _OrderDbContaxt.SaveChanges();


            var cartItens = _OrderDbContaxt.carts.Where(x => x.CustomeruId == order.UserId);
            foreach (var item in cartItens)
            {
                var orderDetails = new OrderDetel()
                {
                    Price =item.price,
                    OrderTotel = item.TotelAmount,
                    Quantity = item.Quantity,
                    ProductId =item.ProdectId,
                    OrderTd = order.Id,
                };
                _OrderDbContaxt.orderDetels.Add(orderDetails);
            }
            _OrderDbContaxt.SaveChanges();
            _OrderDbContaxt.carts.RemoveRange(cartItens);
            _OrderDbContaxt.SaveChanges();
            return Ok(new { OrderTd = order.Id});


        }
        //::::::::::::::::::: E :::::::::::::::::::


        //::::::::::::::::::: S :::::::::::::::::::
        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public IActionResult PindingOrders()
        {
            var orders = _OrderDbContaxt.orders.Where(x => x.IsComplet == false);
            return Ok(orders);

        }
        //::::::::::::::::::: E :::::::::::::::::::


        //::::::::::::::::::: S :::::::::::::::::::
        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public IActionResult Completed()
        {
            var orders = _OrderDbContaxt.orders.Where(x => x.IsComplet == true);
            return Ok(orders);
        }
        //::::::::::::::::::: E :::::::::::::::::::


        //::::::::::::::::::: S :::::::::::::::::::
        [HttpGet("[action]/{orderId}")]
        public IActionResult OrderDetals(int orderId)
        {
            var order = _OrderDbContaxt.orders.Where(x => x.Id == orderId)
                .Include(y => y.orderDetels)
                .ThenInclude(z =>z.productss);
            return Ok(order);
        }
        //::::::::::::::::::: E :::::::::::::::::::


        //::::::::::::::::::: S :::::::::::::::::::
        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public IActionResult Totelorders()
        {
            var orders = (from order in _OrderDbContaxt.orders
                          where order.IsComplet == false
                          select order.IsComplet).Count();

            return Ok(new {PindingOrders =orders});
        }
        //::::::::::::::::::: E :::::::::::::::::::


        //::::::::::::::::::: S :::::::::::::::::::
        [HttpGet("[action]/{userId}")]
        public IActionResult Orderbyuser(int userId)
        {
            var orders = _OrderDbContaxt.orders.Where(x => x.UserId == userId).OrderByDescending(x => x.orderDetels);
            return Ok(orders);
        }
        //::::::::::::::::::: E :::::::::::::::::::


        //::::::::::::::::::: S :::::::::::::::::::
        [Authorize(Roles = "Admin")]
        [HttpPut("[action]/{orderId}")]
        public IActionResult MarkOrderCompleted(int orderId, [FromBody] Order order)
        {
            var orderfromDatabeas = _OrderDbContaxt.orders.Find(orderId);
            if(orderfromDatabeas == null)
            {
                return NotFound("Not Found");
            }
            else
            {
                orderfromDatabeas.IsComplet = order.IsComplet;
                _OrderDbContaxt.SaveChanges();
                return Ok("Order Completed");
            }

        }
        //::::::::::::::::::: E :::::::::::::::::::

    }
}
