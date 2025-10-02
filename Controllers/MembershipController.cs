using Microsoft.AspNetCore.Mvc;
using CSE325_Team12_Project.Models;
using CSE325_Team12_Project.Data;

namespace CSE325_Team12_Project.Controllers
{
    public class MembershipController : Controller
    {
        private readonly AppDbContext _context;
        public MembershipController(AppDbContext context) => _context = context;

        public IActionResult Index() => View(_context.Memberships.ToList());

        public IActionResult Join(Guid userId, Guid troupeId)
        {
            var membership = new Membership
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TroupeId = troupeId,
                JoinedAt = DateTime.UtcNow
            };
            _context.Memberships.Add(membership);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Leave(Guid userId, Guid troupeId)
        {
            var membership = _context.Memberships
                .FirstOrDefault(m => m.UserId == userId && m.TroupeId == troupeId);
            if (membership != null)
            {
                _context.Memberships.Remove(membership);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
