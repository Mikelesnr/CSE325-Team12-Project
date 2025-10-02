using Microsoft.AspNetCore.Mvc;
using CSE325_Team12_Project.Models;
using CSE325_Team12_Project.Data;

namespace CSE325_Team12_Project.Controllers
{
    public class MessageController : Controller
    {
        private readonly AppDbContext _context;
        public MessageController(AppDbContext context) => _context = context;

        public IActionResult Index() => View(_context.Messages.ToList());

        public IActionResult Send(Guid senderId, string content, Guid? troupeId, Guid? conversationId)
        {
            var message = new Message
            {
                Id = Guid.NewGuid(),
                SenderId = senderId,
                Content = content,
                TroupeId = troupeId,
                ConversationId = conversationId,
                CreatedAt = DateTime.UtcNow
            };
            _context.Messages.Add(message);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(Guid id)
        {
            var message = _context.Messages.Find(id);
            if (message != null)
            {
                _context.Messages.Remove(message);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
