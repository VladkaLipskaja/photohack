using System.Threading.Tasks;

namespace Photohack.Services
{
    public interface IEmotionService
    {
        Task<int> GetEmotionAsync(string text);
    }
}
