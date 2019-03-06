using Microsoft.AspNetCore.Mvc;

namespace OcelotApiGw.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        public IActionResult Get() {
            return Ok("ok");
        }
    }
}