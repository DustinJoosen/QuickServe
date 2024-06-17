using Microsoft.AspNetCore.Mvc;

namespace QuickServe.Controllers
{
    [Route("health")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Health()
        {
            return Ok("QuickServe Rest API is running");
        }

    }
}
