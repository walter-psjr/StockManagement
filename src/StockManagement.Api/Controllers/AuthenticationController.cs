using Microsoft.AspNetCore.Mvc;
using StockManagement.Api.Helpers;

namespace StockManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly TokenHelper _tokenHelper;

        public AuthenticationController(TokenHelper tokenHelper)
        {
            _tokenHelper = tokenHelper;
        }

        /// <summary>
        /// Generates a jwt for authentication (for testing purposes only)
        /// </summary>
        /// <returns></returns>
        [HttpPost("token")]
        public IActionResult GenerateToken()
        {
            var token = _tokenHelper.GenerateAccessToken();

            return Ok(new { Token = token });
        }
    }
}