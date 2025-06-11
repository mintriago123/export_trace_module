using Microsoft.AspNetCore.Mvc;

namespace ExportModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DebugController : ControllerBase
    {
        [HttpGet("headers")]
        public IActionResult DebugHeaders()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            return Ok(new { authHeader });
        }
    }
}