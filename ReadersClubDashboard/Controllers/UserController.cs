using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadersClubCore.Data;
using ReadersClubCore.Models;

namespace ReadersClubDashboard.Controllers
{
    public class UserController : Controller
    {
        private readonly ReadersClubContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ReadersClubContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
           var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

    }
}
