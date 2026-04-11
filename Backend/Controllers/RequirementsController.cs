using Microsoft.AspNetCore.Mvc;
using Backend.Services;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequirementsController : ControllerBase
    {
        private readonly RequirementService _service;

        public RequirementsController(RequirementService service)
        {
            _service = service;
        }

        [HttpGet("test")]
        public async Task<IActionResult> GetTest()
        {
            var result = await _service.TestConnectionAsync();
            return Ok(result);
        }
    }
}