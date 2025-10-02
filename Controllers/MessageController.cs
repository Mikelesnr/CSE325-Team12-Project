using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CSE325_Team12_Project.Models;
using CSE325_Team12_Project.Data;

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

        // GET: api/message
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var messages = await _context.Messages.ToListAsync();
            return Ok(messages);
        }

        // POST: api/message/send
        [HttpPost("send")]
        [Authorize]
        public async Task<IActionResult> Send([FromBody] SendMessageRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var message = new Message
                {
                    Id = Guid.NewGuid(),
                    SenderId = request.SenderId,
                    Content = request.Content,
                    TroupeId = request.TroupeId,
                    ConversationId = request.ConversationId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                 return Ok(new { message = "Message sent successfully.", data = message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while sending the message." });
            }
        }

        // DELETE: api/message/{id}
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

                return Ok(new { message = "Message sent successfully.", data = message });

            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the message." });
            }
        }
    }

    public class SendMessageRequest
    {
        public Guid SenderId { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid? TroupeId { get; set; }
        public Guid? ConversationId { get; set; }
    }
}
