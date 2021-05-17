using Microsoft.AspNetCore.Mvc;
using StockManagement.Api.Helpers;

namespace StockManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly TokenHelper _tokenHelper;

        public AuthenticationController(TokenHelper tokenHelper)
        {
            _tokenHelper = tokenHelper;
        }

        [HttpPost("token")]
        public IActionResult GenerateToken()
        {
            var token = _tokenHelper.GenerateAccessToken();

            return Ok(new { Token = token });
        }
    }
}