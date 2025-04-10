using Microsoft.AspNetCore.Mvc;

namespace ReadersClubDashboard.Controllers
{
    public class ReviewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        // صفحة التعليقات
        public IActionResult Comments()
        {
            return View();
        }
    }
}
