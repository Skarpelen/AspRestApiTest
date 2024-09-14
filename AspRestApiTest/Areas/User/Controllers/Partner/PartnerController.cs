using Microsoft.AspNetCore.Mvc;

namespace AspRestApiTest.Areas.User.Controllers.Partner
{
    [Area("User")]
    [Route("api/[area]/[controller]")]
    public class PartnerController : Controller
    {
        [HttpPost("rememberMe")]
        public IActionResult RememberMe([FromQuery] string code)
        {
            return Ok(new { Message = "Remembered successfully", Code = code });
        }
    }
}
