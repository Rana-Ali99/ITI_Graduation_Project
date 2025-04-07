using Microsoft.AspNetCore.Mvc;

namespace ReadersClubDashboard.Controllers
{
    public class StoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // صفحة إضافة رواية
        public IActionResult AddStory()
        {
            return View();
        }

        // صفحة عرض الروايات
        public IActionResult Stories()
        {
            return View();
        }
    }
}
