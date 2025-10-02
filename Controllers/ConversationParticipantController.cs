using Microsoft.AspNetCore.Mvc;
using CSE325_Team12_Project.Models;
using CSE325_Team12_Project.Data;

namespace CSE325_Team12_Project.Controllers
{
    public class ConversationParticipantController : Controller
    {
        private readonly AppDbContext _context;
        public ConversationParticipantController(AppDbContext context) => _context = context;

        public IActionResult Index() => View(_context.ConversationParticipants.ToList());

        public IActionResult Add(Guid conversationId, Guid userId)
        {
            var participant = new ConversationParticipant
            {
                Id = Guid.NewGuid(),
                ConversationId = conversationId,
                UserId = userId,
                JoinedAt = DateTime.UtcNow
            };
            _context.ConversationParticipants.Add(participant);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Remove(Guid conversationId, Guid userId)
        {
            var participant = _context.ConversationParticipants
                .FirstOrDefault(p => p.ConversationId == conversationId && p.UserId == userId);
            if (participant != null)
            {
                _context.ConversationParticipants.Remove(participant);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
