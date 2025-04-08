using Microsoft.AspNetCore.Mvc;

namespace ReadersClubDashboard.Controllers
{
    public class CategoryController : Controller
    {
        ReadersClubCore.Data.ReadersClubContext contaxt = new ReadersClubCore.Data.ReadersClubContext();
        public IActionResult Index()
        {

            return View();
        }
    }
}
