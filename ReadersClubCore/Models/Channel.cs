using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadersClubCore.Models
{
    public class Channel : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [RegularExpression(@"^.*\.(jpg|jpeg|png)$",
      ErrorMessage = "Only image files (jpg, jpeg, png) are allowed.")]
        public string? Image { get; set; }
        public ICollection<Story> Stories { get; set; } = new List<Story>();
    }
}
