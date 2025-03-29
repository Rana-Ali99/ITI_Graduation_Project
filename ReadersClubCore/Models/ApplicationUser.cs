using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadersClubCore.Models
{
    public class ApplicationUser:IdentityUser<int>
    {
        [MaxLength(50)]
        public string Name { get; set; }  //Regex for name
        public string Image { get; set; }
    }
}
