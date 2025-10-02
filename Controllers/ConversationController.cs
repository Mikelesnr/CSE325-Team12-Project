using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CSE325_Team12_Project.Data;
using CSE325_Team12_Project.Models;

//The "Conversation" and "ConversationParticipant" will show a "notFound Error" because
// the "ConversationModel" has not been created yet, it should get solved by itself once
// "ConversationModel" is created (delete this comment after creating the Model)

namespace CSE325_Team12_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ConversationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Start a new conversation with the given users.
        /// </summary>
        [HttpPost("start")]
        [Authorize]
        public async Task<IActionResult> StartConversation([FromBody] StartConversationRequest request)
        {
            try
            {
                if (request.UserIds == null || request.UserIds.Count < 2)
                {
                    return BadRequest(new { message = "A conversation requires at least two users." });
                }

                // Create new conversation
                var conversation = new Conversation
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Participants = request.UserIds.Select(uid => new ConversationParticipant
                    {
                        Id = Guid.NewGuid(),
                        UserId = uid,
                        JoinedAt = DateTime.UtcNow
                    }).ToList()
                };

                _context.Conversations.Add(conversation);
                await _context.SaveChangesAsync();

                return Ok(new { conversation.Id });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error creating conversation." });
            }
        }

        /// <summary>
        /// Get details of a specific conversation.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetConversation(Guid id)
        {
            try
            {
                var conversation = await _context.Conversations
                    .Include(c => c.Participants)
                    .ThenInclude(p => p.User)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (conversation == null)
                    return NotFound();

                return Ok(new
                {
                    conversation.Id,
                    conversation.CreatedAt,
                    participants = conversation.Participants.Select(p => new
                    {
                        p.User.Id,
                        p.User.Name,
                        p.User.Email,
                        p.User.AvatarUrl
                    })
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error fetching conversation." });
            }
        }

        /// <summary>
        /// List all conversations of the current logged-in user.
        /// </summary>
        [HttpGet("mine")]
        [Authorize]
        public async Task<IActionResult> ListConversations()
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return Unauthorized();
                }

                var conversations = await _context.ConversationParticipants
                    .Include(cp => cp.Conversation)
                    .ThenInclude(c => c.Participants)
                    .ThenInclude(p => p.User)
                    .Where(cp => cp.UserId == userId)
                    .Select(cp => cp.Conversation)
                    .ToListAsync();

                return Ok(conversations.Select(c => new
                {
                    c.Id,
                    c.CreatedAt,
                    participants = c.Participants.Select(p => new
                    {
                        p.User.Id,
                        p.User.Name,
                        p.User.AvatarUrl
                    })
                }));
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error fetching conversations." });
            }
        }
    }

    // Request DTO
    public class StartConversationRequest
    {
        public List<Guid> UserIds { get; set; } = new();
    }
}
