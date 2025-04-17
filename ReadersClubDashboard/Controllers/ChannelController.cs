using Microsoft.AspNetCore.Mvc;
using ReadersClubCore.Data;
using ReadersClubCore.Models;
using static System.Net.Mime.MediaTypeNames;
namespace ReadersClubDashboard.Controllers
{
    //handling channels
    public class ChannelController : Controller
    {
        
        private readonly ReadersClubContext _context;
        public ChannelController(ReadersClubContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var channels = _context.Channels.ToList();
            int numberOfChannels = _context.Channels.Count();
            var numberOfDeletedChannels = _context.Channels.Where(c => c.IsDeleted == true).Count();
            ViewData["NumberOfChannels"] = numberOfChannels - numberOfDeletedChannels;

            if (channels.Any())
                return View(channels);
            else
                return View("NoChannels");
        }

        [HttpGet]
        public IActionResult AddChannel()
        {
            return View("AddChannel");
        }

        [HttpPost]
        public async Task<IActionResult> AddChannel(Channel channelData, IFormFile imageFile)
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
            if (ModelState.IsValid)
            {
                _context.Channels.Add(channelData);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("AddChannel", channelData); // لو فيه error في الفاليديشن
        }

        [HttpGet]
        public RedirectToActionResult DeleteChannel(int id)
        {
            // make softe delete using isDeleted property
            var channel = _context.Channels.Find(id);
            if (channel != null)
            {
                channel.IsDeleted = true;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult EditeChannel(int id)
        {
            var channel = _context.Channels.Find(id);
            return View(channel);
        }

        [HttpPost]
        public IActionResult EditeChannel(int id, Channel channelData, IFormFile imageFile)
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

            _context.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
