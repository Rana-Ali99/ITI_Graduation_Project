using Microsoft.EntityFrameworkCore;
using ReadersClubCore.Data;
using ReadersClubCore.Models;
using ReadersClubDashboard.ViewModels;

namespace ReadersClubDashboard.Service
{
    public class StoryService
    {
        private readonly ReadersClubContext _context;

        public StoryService(ReadersClubContext context)
        {
            _context = context;
        }
        #region Methods For Both

        #endregion

        #region Methods For Admin
        // القصص الأكثر مشاهده للأدمن
        public async Task<IEnumerable<Story>> MostViewedStories()
        {
            var stories = await _context.Stories
                .Where(x => x.IsDeleted == false
                && x.IsValid == true
                && x.ViewsCount > 100)
                .OrderByDescending(x => x.ViewsCount)
                .ToListAsync();
            return stories;
        }
        // القصص الأكثر تقييما للأدمن
        public async Task<IEnumerable<Story>> MostRatedStories()
        {
            var stories = await _context.Stories
                .Where(x => x.IsValid == true
                && x.IsDeleted == false)
                .Select(x => new StoryVM
                {
                    Story = x,
                    AverageRating = x.Reviews.Average(r => r.Rating)
                })
                .OrderByDescending(x => x.AverageRating)
                .Select(x => x.Story)
                .ToListAsync();
            return stories;
        }

        #endregion

        #region Methods For Author

        // القصص الأكثر مشاهده للكاتب
        public async Task<IEnumerable<Story>> MostViewedStories(int userId)
        {
            var stories = await _context.Stories
                .Where(x => x.IsDeleted == false
                && x.IsValid == true
                && x.UserId == userId
                && x.ViewsCount > 100)
                .OrderByDescending(x => x.ViewsCount)
                .ToListAsync();
            return stories;
        }
        // القصص الأكثر تقييما للكاتب
        public async Task<IEnumerable<StoryVM>> MostRatedStories(int userId)
        {
            var stories = await _context.Stories
                .Where(x => x.IsValid == true
                && x.IsDeleted == false 
                && x.UserId == userId)
                .Select(x => new StoryVM
                {
                    Story = x,
                    AverageRating = x.Reviews.Average(r => r.Rating)
                })
                .OrderByDescending(x => x.AverageRating)
                .ToListAsync();
            return stories;
        }
    }
    #endregion

    
}
