using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadersClubCore.Data;
using ReadersClubCore.Models;
using ReadersClubCore.ViewModels;

namespace ReadersClub.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ReadersClubContext _context;

        public ReviewsController(ReadersClubContext context)
        {
            _context = context;
        }

        // GET: /Review/
        public async Task<IActionResult> Index()
        {
            var reviews = new List<ReviewViewModel>
    {
        new ReviewViewModel
        {
            Id = 1,
            UserName = "نور علي",
            StoryTitle = "العاصفة",
            Comment = "رواية ممتعة وتستحق القراءة",
            Rating = 4
        },
        new ReviewViewModel
        {
            Id = 2,
            UserName = "سارة محمد",
            StoryTitle = "الظلال السوداء",
            Comment = "لم تعجبني النهايات المفتوحة.",
            Rating = 2
        }
            };
            return View(reviews);


            //var reviews = await _context.Reviews
            //    .Include(r => r.User)
            //    .Include(r => r.Story)
            //    .Select(r => new ReviewViewModel
            //    {
            //        Id = r.Id,
            //        Comment = r.Comment,
            //        Rating = r.Rating,
            //        StoryTitle = r.Story.Title,
            //        UserName = r.User.Name
            //    })
            //    .ToListAsync();

            //return View(reviews);
        }

        // GET: /Review/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
                return NotFound();

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
