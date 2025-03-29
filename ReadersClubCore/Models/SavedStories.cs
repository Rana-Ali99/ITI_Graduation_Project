using System.ComponentModel.DataAnnotations.Schema;

namespace ReadersClubCore.Models
{
    public class SavedStories:BaseEntity
    {
        public int StoryId { get; set; }
        public Story Story { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public bool IsSaved { get; set; }
    }
    
}
