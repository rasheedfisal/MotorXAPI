using MotorX.Api.Utils;
using System.Net.Http.Headers;

namespace MotorX.Api.Services
{
    public class ImageProcessor : IImageProcessor
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        public ImageProcessor(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }
        public async Task<bool> RemoveImageAsync(string filePath)
        {
            string imgToDeletePath = Path.Combine(_hostEnvironment.ContentRootPath, filePath);

            if (File.Exists(imgToDeletePath))
            {
                //System.IO.File.Delete(imgToDeletePath);
                using FileStream stream = new FileStream(filePath, FileMode.Truncate, FileAccess.Write, FileShare.Delete, 4096, true);

                await stream.FlushAsync();
                File.Delete(filePath);
                return true;
            }
            return false;
        }

        public async Task<string?> SaveImageAsync(IFormFile? file)
        {
            try
            {
                if (file is not null)
                {
                    var folderName = Path.Combine("Resources", "Images");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    string? dbPath = null;
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName!.Trim('"');
                        //var fileExt = Path.GetExtension(fileName);

                        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";

                        var fullPath = Path.Combine(pathToSave, uniqueFileName);

                        dbPath = Path.Combine(folderName, uniqueFileName);
                        var originalFile = file.OpenReadStream();
                        using FileStream? outputFile = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, 4096, true);
                        //await file.CopyToAsync(stream);
                        // var imageUtil = new ImageUtil(stream);
                        await Resize.SaveImage(originalFile, 600, 400, true, outputFile);

                    }
                    return dbPath;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
    }
}
