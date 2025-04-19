using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadersClubCore.Models;
using ReadersClubDashboard.Helper;
using ReadersClubDashboard.Service;
using ReadersClubDashboard.Sevice;
using ReadersClubDashboard.ViewModels;

namespace ReadersClubDashboard.Controllers
{
    public class StoryController : Controller
    {
        private readonly StoryService storyService;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CategoryService categoryService;
        private readonly ChannelService channelService;
        private readonly IMapper _mapper;

        public StoryController(StoryService _storyService
            , IWebHostEnvironment env
            , UserManager<ApplicationUser> userManager
            ,CategoryService categoryService
            ,ChannelService channelService
            ,IMapper mapper)
        {
            storyService = _storyService;
            _env = env;
            _userManager = userManager;
            this.categoryService = categoryService;
            this.channelService = channelService;
            _mapper = mapper;
        }

        #region Actions For both
        [Authorize]
        public async Task<IActionResult> AddStory()
        {
            CreateStoryForm storyForm = new CreateStoryForm();
            storyForm.Categories = await categoryService.GetAllAsync();
            if (User.IsInRole("admin"))
            {
                storyForm.Channels = await channelService.GetAllChannels();

            }
            else
            {
                var author = await _userManager.FindByNameAsync(User.Identity.Name);
                storyForm.Channels = await channelService.GetAuthorChannels(author.Id);
            }
            storyForm.ApplicationUsers = await _userManager.Users
                .ToListAsync();
            return View(storyForm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddStory(CreateStoryForm model)
        {
            if (ModelState.IsValid)
            {
                if (model.coverFile != null)
                {
                   model.Cover = Guid.NewGuid().ToString() + Path.GetExtension(model.coverFile.FileName);
                    model.Cover = FileSettings.UploadFile(model.coverFile, "Covers", _env.WebRootPath);
                }

                if (model.pdfFile != null)
                {
                    model.File = Guid.NewGuid().ToString() + Path.GetExtension(model.pdfFile.FileName);
                    model.File = FileSettings.UploadFile(model.pdfFile, "pdfs", _env.WebRootPath);
                }

                if (model.audioFile != null)
                {
                    model.Audio = Guid.NewGuid().ToString() + Path.GetExtension(model.audioFile.FileName);
                    model.Audio = FileSettings.UploadFile(model.audioFile, "audios", _env.WebRootPath);
                }
                if (User.IsInRole("admin"))
                {
                    //filter with isvalid & isactive
                    model.IsValid = true;
                }else
                {
                    model.IsValid = false;
                    var user = User.Identity.Name;
                    var auhor = await _userManager.FindByNameAsync(user);
                    model.UserId = auhor.Id;
                }
                var story = new Story()
                {
                    Title = model.Title,
                    Audio = model.Audio,
                    File = model.File,
                    Cover = model.Cover,
                    Description = model.Description,
                    Summary = model.Summary,
                    IsValid = model.IsValid,
                    CategoryId = model.CategoryId,
                    UserId = model.UserId,
                    ChannelId = model.ChannelId,
                };
                storyService.AddStory(story);
                return RedirectToAction("Stories");
            }
            model.Categories = await categoryService.GetAllAsync();
            model.Channels = await channelService.GetAllChannels();
            model.ApplicationUsers = await _userManager.Users
                .ToListAsync();
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> UpdateStory(int id)
        {
            var story = storyService.GetStoryById(id);
            if (story == null)
            {
                return NotFound();
            }
            var mappedStory = _mapper.Map<Story, EditStoryForm>(story);
            mappedStory.Categories = await categoryService.GetAllAsync();
            if(User.IsInRole("admin"))
            {
                mappedStory.Channels = await channelService.GetAllChannels();
            }
            else
            {
                var author = await _userManager.FindByNameAsync(User.Identity.Name);
                mappedStory.Channels = await channelService.GetAuthorChannels(author.Id);
            }
            mappedStory.ApplicationUsers = await _userManager.Users
                .ToListAsync();
            return View(mappedStory);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateStory(EditStoryForm model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if(model.coverFile != null)
                    {
                        FileSettings.DeleteFile("Covers",model.Cover);
                        model.Cover = Guid.NewGuid().ToString() + Path.GetExtension(model.coverFile.FileName);
                        model.Cover = FileSettings.UploadFile(model.coverFile, "Covers", _env.WebRootPath);
                    }
                    if(model.pdfFile != null)
                    {
                        FileSettings.DeleteFile("pdfs", model.File);
                        model.File = Guid.NewGuid().ToString() + Path.GetExtension(model.pdfFile.FileName);
                        model.File = FileSettings.UploadFile(model.pdfFile, "pdfs", _env.WebRootPath);
                    }
                    if (model.audioFile != null)
                    {
                        FileSettings.DeleteFile("audios", model.Cover);
                        model.Audio = Guid.NewGuid().ToString() + Path.GetExtension(model.audioFile.FileName);
                        model.Audio = FileSettings.UploadFile(model.audioFile, "audios", _env.WebRootPath);
                    }
                    var story = _mapper.Map<EditStoryForm, Story>(model);
                    if (User.IsInRole("admin"))
                    {
                        story.IsValid = true;
                    }
                    else { story.IsValid = false; }
                    storyService.UpdateStory(story);
                    if(User.IsInRole("admin"))
                    {
                        return RedirectToAction("Stories");
                    }
                    else
                    {
                        return RedirectToAction("AuthorStories");
                    }
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    
                }
            }
            model.Categories = await categoryService.GetAllAsync();
            if (User.IsInRole("admin"))
            {
                model.Channels = await channelService.GetAllChannels();
            }
            else
            {
                var author = await _userManager.FindByNameAsync(User.Identity.Name);
                model.Channels = await channelService.GetAuthorChannels(author.Id);
            }
            model.ApplicationUsers = await _userManager.Users
                .ToListAsync();
            return View(model);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var story =  storyService.GetStoryById(id);
            if (story == null)
            {
                return NotFound();
            }
            story.IsDeleted = true;
            storyService.UpdateStory(story);
            return RedirectToAction("Stories");
        }

        #endregion
        #region Actions For Admin
        //Get All Stories
        [Authorize(Roles = "admin")]
        public IActionResult Stories()
        {
            var stories = storyService.GetAllStories();
            return View("Stories", stories);
        }
        //Most Viewed Stories for Admin
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> MostViewedStories()
        {
            var stories = await storyService.MostViewedStories();
            return View("Stories",stories);
        }
        //Most Rated Stories for Admin
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> MostRatedStories()
        {
            var stories = await storyService.MostRatedStories();
            return View(stories);
        }
        #endregion
        #region Actions For Author
        //Get All Stories for this author
        [Authorize(Roles = "author")]
        public async Task<IActionResult> AuthorStories()
        {
            var user = User.Identity.Name;
            var author = await _userManager.FindByNameAsync(user);
            var stories = storyService.GetAllStories(author.Id);
            return View(stories);
        }
        //Most Viewed Stories for Author
        [Authorize(Roles = "author")]
        public async Task<IActionResult> MostViewedAuthorStories()
        {
            var user = User.Identity.Name;
            var author = await _userManager.FindByNameAsync(user);
            var stories = await storyService.MostViewedStories(author.Id);
            return View(stories);
        }
        //Most Rated Stories for Author
        [Authorize(Roles = "author")]
        public async Task<IActionResult> MostRatedAuthorStories()
        {
            var user = User.Identity.Name;
            var author = await _userManager.FindByNameAsync(user);
            var stories = await storyService.MostRatedStories(author.Id);
            return View(stories);
        }

        #endregion
    }
}
//Edit for story  --nagham
//Handle View of Most Rated and Viewed 
//Action of Story Reviews of author
//Action (Confirm Stories) for admin
//Action show authors stories
//Edit style of dashboard
//transform to arabic
//Details for story
