using Microsoft.AspNetCore.Mvc;
using Backend.Services;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly RequirementService _service;
        private readonly ApplicationDbContext _context;

        public ProjectsController(RequirementService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();
            return project;
        }

        [HttpPost]
        public async Task<ActionResult<Project>> PostProject([FromBody] Project project)
        {
            ModelState.Remove("Requirements"); 
            ModelState.Remove("Author");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
                
                return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка БД (Post): " + (ex.InnerException?.Message ?? ex.Message));
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, [FromBody] Project project)
        {
            if (id != project.Id) return BadRequest("ID mismatch");

            ModelState.Remove("Client");
            ModelState.Remove("Requirements");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var exists = await _context.Projects.AnyAsync(p => p.Id == id);
            if (!exists) return NotFound();

            try
            {
                _context.Projects.Update(project);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                var innerError = ex.InnerException?.Message ?? ex.Message;
                Console.WriteLine($"[Error] PutProject: {innerError}");
                return StatusCode(500, innerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
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