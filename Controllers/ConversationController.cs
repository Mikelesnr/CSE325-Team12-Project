using Microsoft.AspNetCore.Mvc;
using CSE325_Team12_Project.Models;
using CSE325_Team12_Project.Data;

namespace CSE325_Team12_Project.Controllers
{
    public class ConversationController : Controller
    {
        private readonly AppDbContext _context;
        public ConversationController(AppDbContext context) => _context = context;

        public IActionResult Index() => View(_context.Conversations.ToList());

        public IActionResult Start(Guid creatorId)
        {
            var convo = new Conversation
            {
                Id = Guid.NewGuid(),
                CreatedBy = creatorId,
                CreatedAt = DateTime.UtcNow
            };
            _context.Conversations.Add(convo);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
