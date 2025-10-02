using Microsoft.AspNetCore.Mvc;
using CSE325_Team12_Project.Models;
using CSE325_Team12_Project.Data;

namespace CSE325_Team12_Project.Controllers
{
    public class TroupeController : Controller
    {
        private readonly AppDbContext _context;
        public TroupeController(AppDbContext context) => _context = context;

        public IActionResult Index() => View(_context.Troupes.ToList());

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Troupe troupe)
        {
            troupe.CreatedAt = DateTime.UtcNow;
            _context.Troupes.Add(troupe);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(Guid id) => View(_context.Troupes.Find(id));

        [HttpPost]
        public IActionResult Edit(Troupe troupe)
        {
            _context.Troupes.Update(troupe);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(Guid id)
        {
            var troupe = _context.Troupes.Find(id);
            if (troupe != null)
            {
                _context.Troupes.Remove(troupe);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
