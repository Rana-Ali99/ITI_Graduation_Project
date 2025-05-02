using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadersClubApi.DTO;
using ReadersClubCore.Data;
using ReadersClubCore.Models;

namespace ReadersClubApi.Controllers
{

    public class StoriesController : BaseController
    {
        private readonly ReadersClubContext _context;

        public StoriesController(ReadersClubContext context)
        {
            _context = context;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StoryVM>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllStories()
        {
            var stories = _context.Stories
                .Include(c => c.Category)
                .Include(c => c.Channel)
                .Where(x => !x.IsDeleted
                        && x.IsValid
                        && x.IsActive
                        && x.Status == Status.Approved)
                .Select(x => new StoryVM
                {
                    Story = x,
                    AverageRating = x.Reviews.Any() ? x.Reviews.Average(r => r.Rating) : 0
                })
                .ToList();

            if (!stories.Any())
                return NotFound("No stories found.");

            return Ok(new
            {
                Stories = stories
            });
        }

    }
}
