using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CSE325_Team12_Project.Data;
using CSE325_Team12_Project.Models;
using CSE325_Team12_Project.Services;

namespace CSE325_Team12_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConversationService _conversationService;

        public ConversationsController(ApplicationDbContext context, IConversationService conversationService)
        {
            _context = context;
            _conversationService = conversationService;
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
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var currentUserId))
                    return Unauthorized();

                if (request.UserIds == null || request.UserIds.Count == 0)
                    return BadRequest(new { message = "You must select a participant." });

                // Ensure the current user is included
                if (!request.UserIds.Contains(currentUserId))
                    request.UserIds.Add(currentUserId);

                // Ensure exactly two distinct users
                request.UserIds = request.UserIds.Distinct().ToList();
                if (request.UserIds.Count != 2)
                    return BadRequest(new { message = "A conversation must include exactly two distinct users." });

                var conversation = await _conversationService.CreateConversationAsync(request.UserIds);
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
                    return Unauthorized();

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

    public class StartConversationRequest
    {
        public List<Guid> UserIds { get; set; } = new();
    }
}
