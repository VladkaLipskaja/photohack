using Photohack.Models;
using System.Threading.Tasks;

namespace Photohack.Services
{
    public interface IMusicService
    {
        Task<MusicTrack> GetMusicLinksAsync(string phrase, string words);
    }
}
