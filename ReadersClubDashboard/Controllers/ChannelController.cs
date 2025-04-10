using Microsoft.AspNetCore.Mvc;

namespace ReadersClubDashboard.Controllers
{
    public class ChannelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
