using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        [HttpPost]
        public IActionResult Auth(string username, string password)
        {
            if (username == "ErickGX" && password == "123456")
            {
                var token = TokenService.GenerateToken(new Model.Employee());   
                return Ok(token); 
            }

            return BadRequest("Username ou Password Invalido");
        }
    }
}
