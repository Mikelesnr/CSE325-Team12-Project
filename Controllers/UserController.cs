using Microsoft.AspNetCore.Mvc;
using CSE325_Team12_Project.Models;
using CSE325_Team12_Project.Data;

namespace CSE325_Team12_Project.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context) => _context = context;

        public IActionResult Index() => View(_context.Users.ToList());

        public IActionResult Details(Guid id) => View(_context.Users.Find(id));

        public IActionResult Edit(Guid id) => View(_context.Users.Find(id));

        [HttpPost]
        public IActionResult Edit(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
