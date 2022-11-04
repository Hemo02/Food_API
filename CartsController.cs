using FoodApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly RestDbContaxt _restDbContaxt;

        public CartsController(RestDbContaxt restDbContaxt)
        {
            _restDbContaxt = restDbContaxt;
        }

        //:::::::::::::::: S ::::::::::::::::::::
        [HttpGet("{UserId}")]
        public IActionResult Getuserid(int userid)
        {
            var user = _restDbContaxt.carts.Where(x => x.CustomeruId == userid);
            if(user == null)
            {
                return NotFound();
            }
            var Cartsitems = from a in _restDbContaxt.carts.Where(
                x => x.CustomeruId == userid)
                             join b in _restDbContaxt.productss on a.ProdectId equals b.id
                             select new
                             {
                                 Id = a.id,
                                 prise = a.price,
                                 Toelamonut =a.TotelAmount,
                                 Quantity =a.Quantity,
                                 prodectname = b.Titel,
                             };
            return Ok(Cartsitems);
        }
        //:::::::::::::::: E ::::::::::::::::::::


        //:::::::::::::::: S ::::::::::::::::::::
        [HttpPost]
        public IActionResult Post([FromBody]Cart carts)
        {
            var cart = _restDbContaxt.carts.FirstOrDefault(x => x.ProdectId == carts.ProdectId && x.CustomeruId == carts.CustomeruId);
            if (cart != null)
            {
                cart.Quantity += carts.Quantity;
                cart.TotelAmount = carts.price * cart.Quantity;
            }
            else
            {
                var shopping = new Cart()
                {
                    CustomeruId = carts.CustomeruId,
                    ProdectId = carts.ProdectId,
                    price = carts.TotelAmount,

                };
                _restDbContaxt.carts.Add(shopping);
            }
            _restDbContaxt.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
        //:::::::::::::::: E ::::::::::::::::::::

        //:::::::::::::::: S ::::::::::::::::::::
        [HttpGet("[action]/{userid}")]
        public IActionResult TotelItem(int userid)
        {
            var caritem = (from cart in _restDbContaxt.carts
                           where cart.CustomeruId == userid
                           select cart.Quantity).Sum();
            return Ok(new {TotelItem=caritem});
        }
        //:::::::::::::::: E ::::::::::::::::::::


        //:::::::::::::::: S ::::::::::::::::::::
        [HttpDelete("{userId}")]
        public IActionResult Delete(int userId)
        {
            var cart = _restDbContaxt.carts.Where(x => x.CustomeruId == userId);
            _restDbContaxt.carts.RemoveRange(cart);
            _restDbContaxt.SaveChanges();
            return Ok();
        }
        //:::::::::::::::: E ::::::::::::::::::::


        //:::::::::::::::: S ::::::::::::::::::::
        [HttpGet("[action]/{userId}")]
        public IActionResult TotelAmount(int userId)
        {
            var totalAmount = (from cart in _restDbContaxt.carts
                               where cart.CustomeruId == userId
                               select cart.TotelAmount).Sum();
            return Ok(new { TotelItem = totalAmount });
        }
        //:::::::::::::::: E ::::::::::::::::::::

    }
}
