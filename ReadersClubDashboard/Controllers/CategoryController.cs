using Microsoft.AspNetCore.Mvc;

namespace ReadersClubDashboard.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
