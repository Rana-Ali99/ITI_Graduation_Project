using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadersClubApi.DTO;

namespace ReadersClubApi.Controllers
{
 
    public class SecurityController : BaseController
    {
        //Register
        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegiserForm regiserForm)
        {
            return Ok();
        }
        //Login
        //LogOut
        //Forget Password
    }
}
