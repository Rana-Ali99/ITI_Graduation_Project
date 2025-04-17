using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReadersClubCore.Data;
using ReadersClubCore.Models;
using ReadersClubDashboard.Services;

namespace ReadersClubDashboard.Controllers
{
    public class StoryController : Controller
    {
        private readonly StoryService storyService;
        private readonly IWebHostEnvironment _env;
        public StoryController(StoryService _storyService, IWebHostEnvironment env)
        {
            storyService = _storyService;
            _env = env;
        }
        public IActionResult Stories()
        {
            var stories = storyService.GetAllStories();
            return View(stories);
        }

        public IActionResult AddStory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddStory(Story model, IFormFile coverFile, IFormFile pdfFile, IFormFile audioFile)
        {
            if (ModelState.IsValid)
            {
                if (coverFile != null)
                {
                    var coverPath = Path.Combine("uploads", "covers", Guid.NewGuid() + Path.GetExtension(coverFile.FileName));
                    var fullCoverPath = Path.Combine(_env.WebRootPath, coverPath);
                    Directory.CreateDirectory(Path.GetDirectoryName(fullCoverPath));
                    using (var stream = new FileStream(fullCoverPath, FileMode.Create))
                    {
                        await coverFile.CopyToAsync(stream);
                    }
                    model.Cover = "/" + coverPath.Replace("\\", "/");
                }

                if (pdfFile != null)
                {
                    var pdfPath = Path.Combine("uploads", "pdfs", Guid.NewGuid() + Path.GetExtension(pdfFile.FileName));
                    var fullPdfPath = Path.Combine(_env.WebRootPath, pdfPath);
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPdfPath));
                    using (var stream = new FileStream(fullPdfPath, FileMode.Create))
                    {
                        await pdfFile.CopyToAsync(stream);
                    }
                    model.File = "/" + pdfPath.Replace("\\", "/");
                }

                if (audioFile != null)
                {
                    var audioPath = Path.Combine("uploads", "audios", Guid.NewGuid() + Path.GetExtension(audioFile.FileName));
                    var fullAudioPath = Path.Combine(_env.WebRootPath, audioPath);
                    Directory.CreateDirectory(Path.GetDirectoryName(fullAudioPath));
                    using (var stream = new FileStream(fullAudioPath, FileMode.Create))
                    {
                        await audioFile.CopyToAsync(stream);
                    }
                    model.Audio = "/" + audioPath.Replace("\\", "/");
                }

                storyService.AddStory(model);
                return RedirectToAction("Stories");
            }

            return View(model);
        }



        public IActionResult UpdateStory(int id)
        {
            var story = storyService.GetStoryById(id);
            if (story == null)
            {
                return NotFound();
            }
            return View(story);
        }

        [HttpPost]
        public IActionResult UpdateStory(Story story)
        {
            storyService._context.Update(story);
            storyService._context.SaveChanges();
            return RedirectToAction("Stories");
        }

        public IActionResult Delete(int id)
        {
            var story = storyService.GetStoryById(id);
            if (story == null)
            {
                return NotFound();
            }

            storyService._context.Stories.Remove(story);
            storyService._context.SaveChanges();
            return RedirectToAction("Stories");
        }


    }
}
