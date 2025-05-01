using Microsoft.AspNetCore.Mvc;
using ReadersClubApi.Services;

namespace ReadersClubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        private readonly StoryService _storyService;

        public StoriesController(StoryService storyService)
        {
            _storyService = storyService;
        }

        [HttpGet("popular")]
        public IActionResult GetPopularStories()
        {
            var stories = _storyService.GetMostPopularStories();
            return Ok(stories);
        }
    }
}
