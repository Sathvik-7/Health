using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles="Admin,Health Professional")]
    public class AdminHealthController : ControllerBase
    {

        [HttpGet]
        public IActionResult getData()
        {
            return Ok(new { Message = " Your done"});
        }
    }
}
