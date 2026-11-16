using musicasWEB.API.Services;

namespace musicasWEB.API.Interfaces
{
    public interface IImagemService
    {
        bool IsConfigured { get; }

        Task<string> UploadImageAsync(IFormFile arquivo, ImageFolder pasta);

        Task<bool> DeleteImageAsync(string imageUrl);
    }
}
