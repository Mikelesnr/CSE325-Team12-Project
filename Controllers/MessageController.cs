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
    public class MessageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MessageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/message
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var messages = await _context.Messages.ToListAsync();
            return Ok(messages);
        }

        // ✅ POST: api/message/send
        [HttpPost("send")]
        [Authorize]
        public async Task<IActionResult> Send([FromBody] SendMessageRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if ((request.TroupeId == null && request.ConversationId == null) ||
                    (request.TroupeId != null && request.ConversationId != null))
                {
                    return BadRequest(new { message = "Message must target either a troupe or a conversation, not both." });
                }

                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var senderId))
                    return Unauthorized();

                var sender = await _context.Users.FindAsync(senderId);
                if (sender == null)
                    return Unauthorized();

                var message = new Message
                {
                    Id = Guid.NewGuid(),
                    SenderId = senderId,
                    Content = request.Content,
                    TroupeId = request.TroupeId,
                    ConversationId = request.ConversationId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                return Ok(new MessageDto
                {
                    Id = message.Id,
                    SenderId = message.SenderId,
                    SenderName = sender.Name,
                    Content = message.Content,
                    CreatedAt = message.CreatedAt
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the message.", details = ex.Message });
            }
        }

        // ✅ DELETE: api/message/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var message = await _context.Messages.FindAsync(id);
                if (message == null)
                    return NotFound(new { message = "Message not found." });

                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Message deleted successfully.", data = message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the message." });
            }
        }
    }

    // ✅ DTO used for incoming requests
    public class SendMessageRequest
    {
        public Guid SenderId { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid? TroupeId { get; set; }
        public Guid? ConversationId { get; set; }
    }
}
