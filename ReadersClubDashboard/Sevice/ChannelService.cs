using Microsoft.EntityFrameworkCore;
using ReadersClubCore.Data;
using ReadersClubCore.Models;

namespace ReadersClubDashboard.Sevice
{
    public class ChannelService
    {
        private readonly ReadersClubContext _context;

        public ChannelService(ReadersClubContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Channel>> GetAllChannels()
        {
            return await _context.Channels
                .Where(x => x.IsDeleted == false)
                .ToListAsync();
        }
        public async Task<IEnumerable<Channel>> GetAuthorChannels(int userId)
        {
            return await _context.Channels
                .Where(x => x.IsDeleted == false
                && x.UserId == userId)
                .ToListAsync();
        }
    }
}
