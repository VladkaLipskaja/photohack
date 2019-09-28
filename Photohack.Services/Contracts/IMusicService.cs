using Photohack.Models;
using System.Threading.Tasks;

namespace Photohack.Services
{
    public interface IMusicService
    {
        Task<MusicTrack> GetMusicLinks(string phrase, string words);
    }
}
