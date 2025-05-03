using ReadersClubApi.DTO;

namespace ReadersClubApi.Service
{
    public interface IChannelService
    {
        List<ChannelWithStoriesDto> GetAllChannels();
    }
}
