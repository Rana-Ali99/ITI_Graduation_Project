using ReadersClubCore.Models;

namespace ReadersClubApi.DTO
{
    public class StoryVM
    {
        public Story Story { get; set; }
        public double? AverageRating { get; set; }
    }
}
