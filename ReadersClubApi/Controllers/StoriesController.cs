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
        [HttpGet]
        public IActionResult GetAllStories()
        {
            var stories = _storyService.GetAllStories();
            return Ok(stories);
        }
        [HttpGet("{id}")]
        public IActionResult GetStoryById(int id)
        {
            var story = _storyService.GetStoryById(id);
            if (story == null)
                return NotFound();

            return Ok(story);

        }
    }
}
