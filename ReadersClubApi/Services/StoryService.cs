using Microsoft.EntityFrameworkCore;
using ReadersClubApi.DTO;
using ReadersClubCore.Data;
using System.Linq;

namespace ReadersClubApi.Services
{
    public class StoryService
    {
        private readonly ReadersClubContext _context;

        public StoryService(ReadersClubContext context)
        {
            _context = context;
        }

        public List<PopularStoryDTO> GetMostPopularStories()
        {
            var stories = _context.Stories
                .Include(s => s.Channel)
                .Include(s => s.Category)
                .Where(s => s.IsValid && !s.IsDeleted)
                .Select(s => new PopularStoryDTO
                {
                    Id = s.Id,
                    Title = s.Title,
                    Cover = s.Cover,
                    AverageRating = s.Reviews.Any() ? s.Reviews.Average(r => (float)r.Rating) : 0,
                    ChannelName = s.Channel.Name,
                    CategoryName = s.Category.Name
                })
                .OrderByDescending(s => s.AverageRating)
                .Take(10)
                .ToList();

            return stories;
        }
    }
}
