using Microsoft.AspNetCore.Mvc;
using Backend.Services;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly RequirementService _service;
        private readonly ApplicationDbContext _context;

        public ClientsController(RequirementService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients
                .Include(c => c.Projects)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Clients
                .Include(c => c.Projects)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null) return NotFound();
            return client;
        }

        [HttpPost]
        public async Task<ActionResult<Client>> PostClient([FromBody] Client client)
        {
            ModelState.Remove("Projects");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
                
                return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка БД (Post): " + (ex.InnerException?.Message ?? ex.Message));
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, [FromBody] Client client)
        {
            if (id != client.Id) return BadRequest("ID mismatch");

            ModelState.Remove("Requirements");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var exists = await _context.Clients.AnyAsync(c => c.Id == id);
            if (!exists) return NotFound();

            try
            {
                _context.Clients.Update(client);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                var innerError = ex.InnerException?.Message ?? ex.Message;
                Console.WriteLine($"[Error] PutClient: {innerError}");
                return StatusCode(500, innerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();

            _context.Clients.Remove(client);
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