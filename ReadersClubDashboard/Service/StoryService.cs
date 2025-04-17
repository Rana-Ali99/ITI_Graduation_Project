using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadersClubCore.Data;
using ReadersClubCore.Models;
namespace ReadersClubDashboard.Services
{
    public class StoryService
    {

        public readonly ReadersClubContext _context;
        public StoryService([FromServices] ReadersClubContext context)
        {
            _context = context;
        }

        public List<Story> GetAllStories()
        {
            return _context.Stories.Include(c => c.Category).Include(c => c.Channel).ToList();
        }
        public Story GetStoryById(int id)
        {
            return _context.Stories.Include(c => c.Category).Include(c => c.Channel).FirstOrDefault(s => s.Id == id);
        }

        public void AddStory(Story story)
        {
            story.Category = _context.Categories.FirstOrDefault(c => c.Id == story.CategoryId);
            story.Channel = _context.Channels.FirstOrDefault(c => c.Id == story.ChannelId);
            _context.Stories.Add(story);
            _context.SaveChanges();
        }
        public void UpdateStory(Story story)
        {
            story.Category = _context.Categories.FirstOrDefault(c => c.Id == story.CategoryId);
            story.Channel = _context.Channels.FirstOrDefault(c => c.Id == story.ChannelId);
            _context.Stories.Update(story);
            _context.SaveChanges();
        }
        public void DeleteStory(int id)
        {
            var story = _context.Stories.FirstOrDefault(s => s.Id == id);
            if (story != null)
            {
                _context.Stories.Remove(story);
                _context.SaveChanges();
            }
        }
        public List<Story> GetStoriesByCategory(int categoryId)
        {
            return _context.Stories.Where(s => s.CategoryId == categoryId).ToList();
        }
        public List<Story> GetStoriesByUser(int userId)
        {
            return _context.Stories.Where(s => s.UserId == userId).ToList();
        }
        public List<Story> GetStoriesByChannel(int channelId)
        {
            return _context.Stories.Where(s => s.ChannelId == channelId).ToList();
        }
        public List<Story> GetStoriesByStatus(Status status)
        {
            return _context.Stories.Where(s => s.Status == status).ToList();
        }
        public List<Story> GetStoriesByTitle(string title)
        {
            return _context.Stories.Where(s => s.Title.Contains(title)).ToList();
        }
        public List<Story> GetStoriesByDescription(string description)
        {
            return _context.Stories.Where(s => s.Description.Contains(description)).ToList();
        }

        public List<Story> GetStoriesByIsActive(bool isActive)
        {
            return _context.Stories.Where(s => s.IsActive == isActive).ToList();
        }
        public List<Story> GetStoriesByIsValid(bool isValid)
        {
            return _context.Stories.Where(s => s.IsValid == isValid).ToList();
        }





    }
}
