using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CSE325_Team12_Project.Models;
using CSE325_Team12_Project.Data;

namespace CSE325_Team12_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MembershipController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/membership
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var memberships = await _context.Memberships.ToListAsync();
            return Ok(memberships);
        }

        // POST: api/membership/join
        [HttpPost("join")]
        [Authorize]
        public async Task<IActionResult> Join([FromBody] MembershipRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var membership = new Membership
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    TroupeId = request.TroupeId,
                    JoinedAt = DateTime.UtcNow
                };

                _context.Memberships.Add(membership);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Joined troupe successfully.", membership });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while joining troupe." });
            }
        }

        // DELETE: api/membership/leave
        [HttpDelete("leave")]
        [Authorize]
        public async Task<IActionResult> Leave([FromBody] MembershipRequest request)
        {
            try
            {
                var membership = await _context.Memberships
                    .FirstOrDefaultAsync(m => m.UserId == request.UserId && m.TroupeId == request.TroupeId);

                if (membership == null)
                    return NotFound(new { message = "Membership not found." });

                _context.Memberships.Remove(membership);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Left troupe successfully." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while leaving troupe." });
            }
        }
    }

    public class MembershipRequest
    {
        public Guid UserId { get; set; }
        public Guid TroupeId { get; set; }
    }
}
