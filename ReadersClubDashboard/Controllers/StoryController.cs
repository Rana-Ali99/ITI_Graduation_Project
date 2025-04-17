using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReadersClubCore.Models;
using ReadersClubDashboard.Service;

namespace ReadersClubDashboard.Controllers
{
    public class StoryController : Controller
    {
        private readonly StoryService _storyService;
        private readonly UserManager<ApplicationUser> _userManager;

        public StoryController(StoryService storyService,UserManager<ApplicationUser> userManager)
        {
           _storyService = storyService;
            _userManager = userManager;
        }
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
        #region Actions For both

        #endregion
        #region Actions For Admin
        //Most Viewed Stories for Admin
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> MostViewedStories()
        {
            var stories = await _storyService.MostViewedStories();
            return View(stories);
        }
        //Most Rated Stories for Admin
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> MostRatedStories()
        {
            var stories = await _storyService.MostRatedStories();
            return View(stories);
        }
        #endregion
        #region Actions For Author
        //Most Viewed Stories for Author
        [Authorize(Roles = "author")]
        public async Task<IActionResult> MostViewedAuthorStories()
        {
            var user = User.Identity.Name;
            var author = await _userManager.FindByNameAsync(user);
            var stories = await _storyService.MostViewedStories(author.Id);
            return View(stories);
        }
        //Most Rated Stories for Author
        [Authorize(Roles = "author")]
        public async Task<IActionResult> MostRatedAuthorStories()
        {
            var user = User.Identity.Name;
            var author = await _userManager.FindByNameAsync(user);
            var stories = await _storyService.MostRatedStories(author.Id);
            return View(stories);
        }

        #endregion
    }
}
