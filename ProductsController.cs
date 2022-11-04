using FoodApi.Models;
using ImageUploader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace FoodApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly RestDbContaxt _dbContaxt;
        public ProductsController(RestDbContaxt dbContaxt)
        {
            _dbContaxt = dbContaxt;
        }

        //:::::::::::::::: S ::::::::::::::::::
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_dbContaxt.productss);
        }
        //::::::::::::::::: E ::::::::::::::::::

        //::::::::::::::::: S ::::::::::::::::::
        [HttpGet("{id}")]
        public IActionResult GetId(int id)
        {
            return Ok(_dbContaxt.productss.Find(id));
        }
        //::::::::::::::::: E ::::::::::::::::::

        //::::::::::::::::: S ::::::::::::::::::
        [HttpGet("[action]/{id}")]
        public IActionResult ProductsByid(int Categoryid)
        {
            var products = from x in _dbContaxt.productss
                           where x.CategoryId == Categoryid
                           select new
                           {
                               id = x.id,
                               Titel = x.Titel,
                               Price = x.price,
                               Discription = x.Discription,
                               Categoryid = x.CategoryId,
                               imageurl =x.imageurl,

                           };
            return Ok(products);
        }
        //::::::::::::::::: E ::::::::::::::::::

        //::::::::::::::::: S ::::::::::::::::::
        [HttpGet("[action]")]
        public IActionResult poplar()
        {
            var product = from x in _dbContaxt.productss
                          where x.Ispopular == true
                          select new
                          {
                              id =x.id,
                              Titel =x.Titel,
                              Price =x.price,
                              imageurl = x.imageurl,
                          };
            return Ok(product);
        }
        //::::::::::::::::: E ::::::::::::::::::

        //::::::::::::::::: S ::::::::::::::::::
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult post([FromBody] Products products)
        {
            var stream = new MemoryStream(products.ImageArray);
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.jpg";
            var folder = "wwwroot";
            var response = FilesHelper.UploadImage(stream, folder, file);
            if (!response)
            {
                return BadRequest();
            }
            else
            {
                products.imageurl = file;
                _dbContaxt.productss.Add(products);
                _dbContaxt.SaveChanges();
                return StatusCode(StatusCodes.Status201Created);
            }
        }
        //::::::::::::::::: E ::::::::::::::::::


        //::::::::::::::::: S ::::::::::::::::::
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult put(int id, [FromBody] Products products)
        {
            var entity = _dbContaxt.productss.Find(id);
            if(entity == null)
            {
                return NotFound("Products Not Found");
            }

            var stream = new MemoryStream(products.ImageArray);
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.jpg";
            var folder = "wwwroot";
            var response = FilesHelper.UploadImage(stream, folder, file);
            if (!response)
            {
                return BadRequest();
            }
            else
            {
                entity.CategoryId = products.CategoryId;
                entity.Titel = products.Titel;
                entity.imageurl = file;
                entity.price = products.price;
                entity.Discription = products.Discription;
                _dbContaxt.SaveChanges();
                return Ok("Product Detals Updated");
            }
        }
        //::::::::::::::::: E ::::::::::::::::::

        //::::::::::::::::: S ::::::::::::::::::
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _dbContaxt.productss.Find(id);
            if(product == null)
            {
                return NotFound("product Not Found");
            }
            else
            {
                _dbContaxt.productss.Remove(product);
                _dbContaxt.SaveChanges();
                return Ok("Deleted product");
            }
            
        }


    }
}
