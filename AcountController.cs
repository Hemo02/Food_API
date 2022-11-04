using AuthenticationPlugin;
using FoodApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AuthenticationPlugin;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace FoodApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AcountController : ControllerBase
    {
        private readonly RestDbContaxt _restDbContaxt;
        private IConfiguration _configuration;
        private readonly AuthService _authService;
   

        public AcountController(RestDbContaxt restDbContaxt, IConfiguration configuration , AuthService authService)
        {

            _restDbContaxt = restDbContaxt;
            _configuration = configuration;
            _authService = authService;
        }

        // (((((((((((((((((--------- Add Data To User Class -------)))))))))))))))
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(User user)
        {
            var EmailAlreadyExists = _restDbContaxt.users.SingleOrDefault( x=> x.Email == user.Email);
            if(EmailAlreadyExists != null) return BadRequest("User Eamil Already Exists");

            var usero = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = SecurePasswordHasherHelper.Hash(user.Password),
                Role = "User",

            };
            _restDbContaxt.users.Add(usero);
            await _restDbContaxt.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);

        }

        //::::::::::::::::::: END ::::::::::::::::::::::::

        //::::::::::::::::::: START ::::::::::::::::::::::::
        [HttpPost("{Loingin}")]
        [AllowAnonymous]
        public async Task<IActionResult> Loingin(User userss)
        {
            var Emaile = _restDbContaxt.users.FirstOrDefaultAsync(x => x.Email == userss.Email);
            if (Emaile == null) return StatusCode(StatusCodes.Status404NotFound);
            var passord = userss.Password;
            if (!SecurePasswordHasherHelper.Verify(userss.Password,passord));
            return Unauthorized();
            var Claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,userss.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,userss.Email),
                new Claim(ClaimTypes.Name,userss.Email),
                new Claim(ClaimTypes.Role,userss.Email),
            };
            var token = _authService.GenerateAccessToken(Claims);
            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                token_type = token.TokenType,
                user_Id = userss.Id,
                username =userss.Name,
                Exprisers_in = token.ExpiresIn,
                creation_Time = token.ValidFrom,
                Exception_Time = token.ValidTo
            });

        }
        //::::::::::::::::::: END ::::::::::::::::::::::::



        //internal void SaveGhanges()
        //{
          //  throw new NotImplementedException();
       // }

       // internal object Categories(object categories)
        //{
          //  throw new NotImplementedException();
        //}
    }
}
