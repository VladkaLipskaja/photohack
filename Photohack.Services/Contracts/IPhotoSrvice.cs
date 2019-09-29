using System.Threading.Tasks;

namespace Photohack.Services
{
    public interface IPhotoService
    {
        Task<string[]> ProcessPhoto(int emotion);
        Task SavePhoto(byte[] image, string name);
        Task<string> GetFilter(int filter, string url);
    }
}
