using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadersClubApi.Service;
using ReadersClubApi.Services;

namespace ReadersClubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        private readonly StoryService _storyService;
        private readonly ReviewService _reviewService;

        public StoriesController(StoryService storyService
            ,ReviewService reviewService)
        {
            _storyService = storyService;
            _reviewService = reviewService;
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
        public async Task<IActionResult> GetStoryById(int id)
        {
            var story = _storyService.GetStoryById(id);
            var storyReviews = await _reviewService.GetAllReviewsInStory(story.Id);
            if (story == null)
                return NotFound();
            story.Reviews = storyReviews;
            return Ok(story);

        }


    }
}