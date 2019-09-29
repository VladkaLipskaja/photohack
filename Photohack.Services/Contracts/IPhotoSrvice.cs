using System.Threading.Tasks;

namespace Photohack.Services
{
    public interface IPhotoService
    {
        Task<string[]> ProcessPhoto(int emotion, string name);
        string SavePhoto(byte[] image);
        Task<string[]> GetFilter(int emotion, byte[] bytes);
    }
}
