using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CSE325_Team12_Project.Models;
using CSE325_Team12_Project.Data;

namespace CSE325_Team12_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/user
        [HttpGet]
        // [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            return user is null ? NotFound(new { message = "User not found." }) : Ok(user);
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id, [FromBody] EditUserRequest request)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound(new { message = "User not found." });

                user.Name = request.Name;
                user.Email = request.Email;
                user.AvatarUrl = request.AvatarUrl;
                user.Role = request.Role;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return Ok(new { message = "User updated successfully.", user });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while updating the user." });
            }
        }
    }

    public class EditUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Trouper;
    }
}
