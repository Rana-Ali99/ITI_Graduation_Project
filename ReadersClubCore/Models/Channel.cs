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
        [MaxLength(50)]
        public string Name { get; set; }
        public string Image { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}
