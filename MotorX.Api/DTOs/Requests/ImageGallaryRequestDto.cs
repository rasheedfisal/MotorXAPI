using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Requests
{
    public class ImageGallaryRequestDto
    {
        //public Guid Id { get; set; }
        public Guid CarOfferId { get; set; }
        [Required]
        public IFormFile FileName { get; set; }
    }
}
