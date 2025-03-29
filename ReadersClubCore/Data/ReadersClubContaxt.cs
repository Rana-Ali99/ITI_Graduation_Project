using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReadersClubCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadersClubCore.Data
{
    public class ReadersClubContaxt: IdentityDbContext<ApplicationUser,ApplicationRole,int>
    {
        public ReadersClubContaxt(DbContextOptions<ReadersClubContaxt> options):base(options)
        {
            
        }
        public DbSet<Story> Stories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<ReadingProgress> ReadingProgresses { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<SavedStories> SavedStories { get; set; }
        public DbSet<Subscribtion> Subscribtions { get; set; }

    }
}
