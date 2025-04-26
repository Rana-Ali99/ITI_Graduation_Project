using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadersClubCore.Data;
using ReadersClubCore.Models;
using ReadersClubDashboard.Sevice;
using static System.Net.Mime.MediaTypeNames;
namespace ReadersClubDashboard.Controllers
{
    //handling channels
    [Authorize]
    public class ChannelController : Controller
    {
        
        private readonly ReadersClubContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ChannelService _channelService;

        public ChannelController(ReadersClubContext context
            ,UserManager<ApplicationUser> userManager
            ,ChannelService channelService)
        {
            _context = context;
           _userManager = userManager;
            _channelService = channelService;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var channels = await _channelService.GetAllChannels();
            if (channels.Any())
                return View(channels);
            else
                return View("NoChannels");
        }
        [Authorize(Roles = "author")]
        public async Task<IActionResult> AuthorChannels()
        {
            var user = await _userManager.GetUserAsync(User);
            var channels = await _channelService.GetAuthorChannels(user.Id);
            if (channels.Any())
                return View("Index",channels);
            else
                return View("NoChannels");
        }
        public IActionResult Details(int id)
        {
            var channel = _channelService.GetChannel(id);
            return View(channel);
        }
        [HttpGet]
        public async Task<IActionResult> AddChannel()
        {
            ViewData["Users"] = await _userManager.Users.ToListAsync();
            return View("AddChannel");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddChannel(Channel channelData, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/ChannelsImages");

                //check if the folder exists or not, if not will create it
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // use a unique name for the file to avoid overwriting
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                // save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // set the image path in the channel data
                channelData.Image = "/Uploads/ChannelsImages/" + fileName;
            }

            // save data
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                _context.Channels.Add(channelData);
                await _context.SaveChangesAsync();
                if (User.IsInRole("admin"))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("AuthorChannels");
                }
            }
            ViewData["Users"] = await _userManager.Users.ToListAsync();
            return View("AddChannel", channelData); // لو فيه error في الفاليديشن
        }

        [HttpGet]
        public IActionResult DeleteChannel(int id)
        {
            return Details(id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteChannel(Channel channel)
        {
            try
            {
                _context.Channels.Remove(channel);
                _context.SaveChanges();
                if (User.IsInRole("admin"))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("AuthorChannels");
                }
            }
            catch
            {
                return View(channel);
            }
        }

        public async Task<IActionResult> EditChannel(int id)
        {
            var channel = _channelService.GetChannel(id);
            ViewData["Users"] = await _userManager.Users.ToListAsync();
            return View(channel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditChannel(int id, Channel channelData, IFormFile? imageFile)
        {
            var channel = _context.Channels.Find(id);
            if (imageFile != null && imageFile.Length > 0)
            {

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/ChannelsImages");

                //check if the folder exists or not, if not will create it
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // use a unique name for the file to avoid overwriting
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                // save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyToAsync(stream);
                }

                // set the image path in the channel data
                channel.Image = "/Uploads/ChannelsImages/" + fileName;
            }

            channel.Name = channelData.Name;
            channel.Description = channelData.Description;
            channel.UserId = channelData.UserId;
            _context.Update(channel);
            _context.SaveChanges();
            if (User.IsInRole("admin"))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("AuthorChannels");
            }

        }
    }
}
