using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CSE325_Team12_Project.Models;
using CSE325_Team12_Project.Data;
using CSE325_Team12_Project.Models.DTOs;

namespace CSE325_Team12_Project.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TroupeController : ControllerBase
    {
        private TroupeDto MapToDto(Troupe troupe)
        {
            return new TroupeDto
            {
                Id = troupe.Id,
                Name = troupe.Name,
                Description = troupe.Description,
                Visibility = troupe.Visibility,
                CreatedAt = troupe.CreatedAt,
                CreatedBy = new UserDto
                {
                    Id = troupe.CreatedBy.Id,
                    Name = troupe.CreatedBy.Name
                },
                Members = troupe.Memberships.Select(m => new MemberDto
                {
                    UserId = m.UserId,
                    Name = m.User?.Name ?? "",
                    Email = m.User?.Email ?? "",
                    JoinedAt = m.JoinedAt
                }).ToList(),
                Messages = troupe.Messages.Select(msg => new MessageDto
                {
                    Id = msg.Id,
                    SenderId = msg.SenderId,
                    Content = msg.Content,
                    CreatedAt = msg.CreatedAt
                }).ToList()
            };
        }

        private readonly ApplicationDbContext _context;

        public TroupeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/troupe
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var troupes = await _context.Troupes
                .Include(t => t.CreatedBy)
                .Include(t => t.Memberships)
                .ToListAsync();

            var dtoList = troupes.Select(MapToDto).ToList();
            return Ok(dtoList);
        }

        // GET: api/troupe/{id}
        [HttpGet("{id}")]
        // [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var troupe = await _context.Troupes
                .Include(t => t.CreatedBy)
                .Include(t => t.Memberships)
                .Include(t => t.Messages)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (troupe is null)
                return NotFound(new { message = "Troupe not found." });

            var dto = MapToDto(troupe);
            return Ok(dto);
        }

        // POST: api/troupe
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] TroupeRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var troupe = new Troupe
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    Visibility = request.Visibility,
                    CreatedById = request.CreatedById,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Troupes.Add(troupe);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Troupe created successfully.", troupe });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while creating the troupe." });
            }
        }

        // PUT: api/troupe/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id, [FromBody] TroupeRequest request)
        {
            try
            {
                var troupe = await _context.Troupes.FindAsync(id);
                if (troupe == null)
                    return NotFound(new { message = "Troupe not found." });

                troupe.Name = request.Name;
                troupe.Description = request.Description;
                troupe.Visibility = request.Visibility;

                _context.Troupes.Update(troupe);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Troupe updated successfully.", troupe });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while updating the troupe." });
            }
        }

        // DELETE: api/troupe/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var troupe = await _context.Troupes.FindAsync(id);
                if (troupe == null)
                    return NotFound(new { message = "Troupe not found." });

                _context.Troupes.Remove(troupe);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Troupe deleted successfully." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the troupe." });
            }
        }
    }

    public class TroupeRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TroupeVisibility Visibility { get; set; } = TroupeVisibility.Public;
        public Guid CreatedById { get; set; }
    }
}
