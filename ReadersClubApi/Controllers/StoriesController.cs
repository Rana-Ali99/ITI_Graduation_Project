using Microsoft.AspNetCore.Authorization;
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
     
        [HttpPost("{id}/increase-views")]
        public IActionResult IncreaseViews(int id)
        {
             _storyService.UpdateStoryViewsCount(id);
           
            return Ok();
        }
        [Authorize]
        [HttpPost("{id}/like")]
        public IActionResult LikeStory(int id)
        {
            _storyService.UpdateStoryLikesCount(id);
            return Ok();
        }
        [Authorize]
        [HttpPost("{id}/dislike")]
        public IActionResult DislikeStory(int id)
        {
            _storyService.UpdateStoryDislikesCount(id);
            return Ok();
        }
        [Authorize]
        [HttpPost("{id}/unlike")]
        public IActionResult UnlikeStory(int id)
        {
            _storyService.UpdateStoryUnlikesCount(id);
            return Ok();
        }
        [Authorize]
        [HttpPost("{id}/undislike")]
        public IActionResult UndislikeStory(int id)
        {
            _storyService.UpdateStoryUnDislikesCount(id);
            return Ok();
        }
    }
}