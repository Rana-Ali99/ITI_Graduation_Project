using Microsoft.EntityFrameworkCore;
using ReadersClubApi.DTO;
using ReadersClubCore.Data;
using ReadersClubCore.Models;
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
        public List<StoryDto> GetAllStories()
        {
            var allstories = _context.Stories
                .Include(s => s.Channel)
                .Include(s => s.Category)
                .Where(s => s.IsValid && !s.IsDeleted && s.IsActive && s.Status == Status.Approved)
                .Select(s => new StoryDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    Cover = string.IsNullOrEmpty(s.Cover) ? null :
            $"http://readersclub.runasp.net//Uploads/Covers/{s.Cover}",
                    File = string.IsNullOrEmpty(s.File) ? null :
            $"http://readersclub.runasp.net//Uploads/Files/{s.File}",
                    Audio = string.IsNullOrEmpty(s.Audio) ? null :
            $"http://readersclub.runasp.net//Uploads/Audios/{s.Audio}",
                    Description = s.Description,
                    Summary = s.Summary,
                    AverageRating = s.Reviews.Any() ? s.Reviews.Average(r => (float)r.Rating) : 0,
                    ChannelName = s.Channel.Name,
                    CategoryName = s.Category.Name,
                    ViewsCount = s.ViewsCount,
                    LikesCount = s.LikesCount,
                    DislikesCount = s.DislikesCount,

                })
                 .Distinct()
                .ToList();

            return allstories;
        }
        public StoryDto GetStoryById(int id)
        {
            var story = _context.Stories
                .Include(s => s.Channel)
                .Include(s => s.Category)
                .Where(s => s.IsValid && !s.IsDeleted && s.IsActive && s.Status == Status.Approved)
                .Select(s => new StoryDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    Cover = string.IsNullOrEmpty(s.Cover) ? null :
            $"http://readersclub.runasp.net//Uploads/Covers/{s.Cover}",
                    Description = s.Description,
                    AverageRating = s.Reviews.Any() ? s.Reviews.Average(r => (float)r.Rating) : 0,
                    ChannelName = s.Channel.Name,
                    CategoryName = s.Category.Name,
                    File = string.IsNullOrEmpty(s.File) ? null :
            $"http://readersclub.runasp.net//Uploads/Files/{s.File}",
                    Audio = string.IsNullOrEmpty(s.Audio) ? null :
            $"http://readersclub.runasp.net//Uploads/Audios/{s.Audio}",
                    Summary = s.Summary,
                    ViewsCount = s.ViewsCount,
                    LikesCount = s.LikesCount,
                    DislikesCount = s.DislikesCount,
                })
                .FirstOrDefault(s => s.Id == id);
            return story;
        }
        public void UpdateStoryViewsCount(int storyId)
        {
            var story = _context.Stories.FirstOrDefault(s => s.Id == storyId);
            if (story != null)
            {
                story.ViewsCount++;
                _context.SaveChanges();
            }
        }
        public void UpdateStoryLikesCount(int storyId)
        {
            var story = _context.Stories.FirstOrDefault(s => s.Id == storyId);
            if (story != null)
            {
                story.LikesCount++;
                _context.SaveChanges();
            }
        }
        public void UpdateStoryUnlikesCount(int storyId)
        {
            var story = _context.Stories.FirstOrDefault(s => s.Id == storyId);
            if (story != null)
            {
                story.LikesCount--;
                _context.SaveChanges();
            }
        }
        public void UpdateStoryDislikesCount(int storyId)
        {
            var story = _context.Stories.FirstOrDefault(s => s.Id == storyId);
            if (story != null)
            {
                story.DislikesCount++;
                _context.SaveChanges();
            }
        }
        public void UpdateStoryUnDislikesCount(int storyId)
        {
            var story = _context.Stories.FirstOrDefault(s => s.Id == storyId);
            if (story != null)
            {
                story.DislikesCount--;
                _context.SaveChanges();
            }
        }

    }
}
