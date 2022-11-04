using FoodApi.Models;
using ImageUploader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace FoodApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly RestDbContaxt _Dbcontaxt;

        public CategoriesController(RestDbContaxt Dbcontaxt)
        {
            _Dbcontaxt = Dbcontaxt;
        }


        //::::::::::::::::::: START ::::::::::::::::::::::::
        [HttpGet]
        public IActionResult GetResult()
        {
            var Categories = from a in _Dbcontaxt.Categories
                             select new
                             {
                                 Id = a.id,
                                 Titel = a.Titel,
                                 Imageurl = a.iamgeurl,
                             };
            return Ok(Categories);
        }
        //::::::::::::::::::: END ::::::::::::::::::::::::

        //::::::::::::::::::: START ::::::::::::::::::::::::
        [Authorize(Roles ="Admin")]
        [HttpGet("{id}")]
        public IActionResult GetId (int id)
        {
            var catsgory = (from c in _Dbcontaxt.Categories
                            where c.id == id
                            select new
                            {
                                Id = c.id,
                                Titel = c.Titel,
                                Imageurl = c.iamgeurl,
                            }).FirstOrDefault();
            return Ok(catsgory);
        }
        //::::::::::::::::::: END ::::::::::::::::::::::::

        //::::::::::::::::::: START ::::::::::::::::::::::::
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Post([FromBody]Category databeasfood)
        {
            var stream = new MemoryStream(databeasfood.ImageArray);
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
                databeasfood.iamgeurl = file;
                _Dbcontaxt.Categories.Add(databeasfood);
                _Dbcontaxt.SaveChanges();
                return StatusCode(StatusCodes.Status201Created);
            }
        }

        //::::::::::::::::::: END ::::::::::::::::::::::::

        //::::::::::::::::::: START ::::::::::::::::::::::::
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult put(int id, [FromBody]Category category)
        {
            var entity = _Dbcontaxt.Categories.Find(id);
            if( entity == null)
            {
                return NotFound("Cotegory Not Found");
            }
            var stream = new MemoryStream(category.ImageArray);
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
                entity.Titel = category.Titel;
                entity.iamgeurl = file;
                _Dbcontaxt.SaveChanges();
                entity.Titel = category.Titel;
                return Ok("Category Is Updated");
            }
        }
        //::::::::::::::::::: END ::::::::::::::::::::::::

        //::::::::::::::::::: START ::::::::::::::::::::::::
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delet(int id)
        {
            var category = _Dbcontaxt.Categories.Find(id);
            if(category == null)
            {
                return NotFound("category Not Found");
            }
            else
            {
                _Dbcontaxt.Remove(category);
                _Dbcontaxt.SaveChanges();
                return Ok("category Deleted");
            }

        }
        //::::::::::::::::::: END ::::::::::::::::::::::::
    }
}
