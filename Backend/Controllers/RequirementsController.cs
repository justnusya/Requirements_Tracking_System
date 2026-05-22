using Microsoft.AspNetCore.Mvc;
using Backend.Services;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequirementsController : ControllerBase
    {
        private readonly RequirementService _service;
        private readonly ApplicationDbContext _context;

        public RequirementsController(RequirementService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Requirement>>> GetRequirements()
        {
            return await _context.Requirements.Include(r => r.Project).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Requirement>> GetRequirement(int id)
        {
            var requirement = await _context.Requirements
                .Include(r => r.Project)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (requirement == null) return NotFound();

            var dependentIds = await _service.GetDependentIdsAsync(id);
            requirement.DependentRequirementIds = dependentIds.ToList();

            return requirement;
        }

        [HttpPost]
        public async Task<ActionResult<Requirement>> PostRequirement([FromBody] Requirement req)
        {
            ModelState.Remove("Project");
            ModelState.Remove("Author");
            ModelState.Remove("Status");
            ModelState.Remove("Priority");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var success = await _service.CreateRequirementAsync(req);
                if (!success) return BadRequest("Не вдалося створити вимогу.");
                return Ok(req);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка БД (Post): " + (ex.InnerException?.Message ?? ex.Message));
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequirement(int id, [FromBody] Requirement req)
        {
            if (id != req.Id) return BadRequest("ID mismatch");

            ModelState.Remove("Project");
            ModelState.Remove("Author");
            ModelState.Remove("Status");
            ModelState.Remove("Priority");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var success = await _service.UpdateRequirementAsync(id, req);
                if (!success) return NotFound();
                return NoContent(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка БД (Put): " + (ex.InnerException?.Message ?? ex.Message));
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequirement(int id)
        {
            var success = await _service.DeleteRequirementAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpGet("test")]
        public async Task<IActionResult> GetTest()
        {
            var result = await _service.TestConnectionAsync();
            return Ok(result);
        }
    }
}