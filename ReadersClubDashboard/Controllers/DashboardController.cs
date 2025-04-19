using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReadersClubDashboard.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        // الصفحة الرئيسية للوحة التحكم
        public IActionResult Index()
        {
            return View();
        }


        // صفحة المستخدمين
        public IActionResult Users()
        {
            return View();
        }
    }
}
