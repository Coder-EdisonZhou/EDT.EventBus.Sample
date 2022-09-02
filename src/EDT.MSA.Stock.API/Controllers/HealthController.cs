using Microsoft.AspNetCore.Mvc;

namespace EDT.MSA.Stocking.API.Controllers
{
    [Route("api/Health")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok($"ok");
    }
}