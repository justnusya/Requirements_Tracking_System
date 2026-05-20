using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Будь ласка, заповніть всі поля." });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return NotFound(new { message = "Акаунту з цим імейлом не існує." });
            }

            if (user.Password != request.Password) 
            {
                return BadRequest(new { message = "Неправильний пароль." });
            }

            return Ok(new { 
                id = user.Id, 
                email = user.Email,
                firstName = user.FirstName,
                lastName = user.LastName
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null || 
                string.IsNullOrEmpty(request.Email) || 
                string.IsNullOrEmpty(request.Password) || 
                string.IsNullOrEmpty(request.FirstName) || 
                string.IsNullOrEmpty(request.LastName))
            {
                return BadRequest(new { message = "Будь ласка, заповніть всі обов'язкові поля." });
            }

            var emailExists = await _context.Users.AnyAsync(u => u.Email == request.Email);
            if (emailExists)
            {
                return BadRequest(new { message = "Користувач з цим імейлом вже існує в системі." });
            }

            var newUser = new User
            {
                Email = request.Email,
                Password = request.Password, 
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { 
                id = newUser.Id, 
                email = newUser.Email,
                firstName = newUser.FirstName,
                lastName = newUser.LastName
            });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class RegisterRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}