using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CSE325_Team12_Project.Data;
using CSE325_Team12_Project.Models;
using CSE325_Team12_Project.Services;
using CSE325_Team12_Project.Models.DTOs;

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
        [HttpPost("start")]
        [Authorize]
        public async Task<IActionResult> StartConversation([FromBody] StartConversationRequest request)
        {
            try
            {
                // Get current user ID from claims
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var currentUserId))
                    return Unauthorized();

                // Validate second participant
                if (request.UserIds == null || request.UserIds.Count != 1)
                    return BadRequest(new { message = "You must specify exactly one other participant." });

                var secondUserId = request.UserIds.First();

                // Ensure distinct users
                if (secondUserId == currentUserId)
                    return BadRequest(new { message = "You cannot start a conversation with yourself." });

                // Create conversation with both participants
                var conversation = new Conversation
                {
                    Id = Guid.NewGuid(),
                    CreatedBy = currentUserId,
                    CreatedAt = DateTime.UtcNow,
                    IsGroup = false,
                    Participants = new List<ConversationParticipant>
            {
                new ConversationParticipant
                {
                    ConversationId = Guid.Empty, // will be set below
                    UserId = currentUserId,
                    JoinedAt = DateTime.UtcNow
                },
                new ConversationParticipant
                {
                    ConversationId = Guid.Empty, // will be set below
                    UserId = secondUserId,
                    JoinedAt = DateTime.UtcNow
                }
            }
                };

                // Assign ConversationId to each participant
                foreach (var participant in conversation.Participants)
                {
                    participant.ConversationId = conversation.Id;
                }

                _context.Conversations.Add(conversation);
                await _context.SaveChangesAsync();

                return Ok(new { conversation.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating conversation.", details = ex.Message });
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
                    .Include(c => c.Participants).ThenInclude(p => p.User)
                    .Include(c => c.Messages)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (conversation == null)
                    return NotFound();

                var dto = new ConversationDto
                {
                    Id = conversation.Id,
                    CreatedBy = conversation.CreatedBy,
                    IsGroup = conversation.IsGroup,
                    CreatedAt = conversation.CreatedAt,
                    Participants = conversation.Participants.Select(p => new ConversationParticipantDto
                    {
                        Id = p.Id,
                        UserId = p.UserId,
                        Name = p.User?.Name ?? "",
                        Email = p.User?.Email ?? "",
                        AvatarUrl = p.User?.AvatarUrl ?? "",
                        JoinedAt = p.JoinedAt
                    }).ToList(),
                    Messages = conversation.Messages
                        .OrderBy(m => m.CreatedAt)
                        .Select(m => new MessageDto
                        {
                            Id = m.Id,
                            SenderId = m.SenderId,
                            Content = m.Content,
                            CreatedAt = m.CreatedAt
                        }).ToList()
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching conversation.", details = ex.Message });
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
                    .ThenInclude(c => c.Participants).ThenInclude(p => p.User)
                    .Include(cp => cp.Conversation.Messages)
                    .Where(cp => cp.UserId == userId)
                    .Select(cp => cp.Conversation)
                    .ToListAsync();

                var dtos = conversations.Select(c => new ConversationDto
                {
                    Id = c.Id,
                    CreatedBy = c.CreatedBy,
                    IsGroup = c.IsGroup,
                    CreatedAt = c.CreatedAt,
                    Participants = c.Participants.Select(p => new ConversationParticipantDto
                    {
                        Id = p.Id,
                        UserId = p.UserId,
                        Name = p.User?.Name ?? "",
                        Email = p.User?.Email ?? "",
                        AvatarUrl = p.User?.AvatarUrl ?? "",
                        JoinedAt = p.JoinedAt
                    }).ToList(),
                    Messages = c.Messages
                        .OrderByDescending(m => m.CreatedAt)
                        .Take(1) // Optional: only return latest message
                        .Select(m => new MessageDto
                        {
                            Id = m.Id,
                            SenderId = m.SenderId,
                            Content = m.Content,
                            CreatedAt = m.CreatedAt
                        }).ToList()
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching conversations.", details = ex.Message });
            }
        }
    }

    public class StartConversationRequest
    {
        public List<Guid> UserIds { get; set; } = new();
    }
}
