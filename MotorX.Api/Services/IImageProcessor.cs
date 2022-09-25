namespace MotorX.Api.Services
{
    public interface IImageProcessor
    {
        Task<string?> SaveImageAsync(IFormFile? file);
        Task<bool> RemoveImageAsync(string filePath);
    }
}
