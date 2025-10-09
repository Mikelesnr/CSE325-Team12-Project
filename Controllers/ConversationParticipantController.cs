using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CSE325_Team12_Project.Models;
using CSE325_Team12_Project.Data;

namespace CSE325_Team12_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationParticipantController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ConversationParticipantController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/conversationparticipant
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var participants = await _context.ConversationParticipants.ToListAsync();
            return Ok(participants);
        }

        // POST: api/conversationparticipant/add
        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] AddParticipantRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var participant = new ConversationParticipant
                {
                    Id = Guid.NewGuid(),
                    ConversationId = request.ConversationId,
                    UserId = request.UserId,
                    JoinedAt = DateTime.UtcNow
                };

                _context.ConversationParticipants.Add(participant);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Participant added successfully.", participant });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while adding participant." });
            }
        }

        // DELETE: api/conversationparticipant/remove
        [HttpDelete("remove")]
        [Authorize]
        public async Task<IActionResult> Remove([FromBody] RemoveParticipantRequest request)
        {
            try
            {
                var participant = await _context.ConversationParticipants
                    .FirstOrDefaultAsync(p => p.ConversationId == request.ConversationId && p.UserId == request.UserId);

                if (participant == null)
                    return NotFound(new { message = "Participant not found." });

                _context.ConversationParticipants.Remove(participant);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Participant removed successfully." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while removing participant." });
            }
        }
    }

    public class AddParticipantRequest
    {
        public Guid ConversationId { get; set; }
        public Guid UserId { get; set; }
    }

    public class RemoveParticipantRequest
    {
        public Guid ConversationId { get; set; }
        public Guid UserId { get; set; }
    }
}
