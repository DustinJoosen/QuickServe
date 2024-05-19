using Microsoft.AspNetCore.Mvc;

namespace QuickServe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {

        [HttpGet]
        public IActionResult Health()
        {
            return Ok("QuickServe Rest API is running");
        }
    }
}
