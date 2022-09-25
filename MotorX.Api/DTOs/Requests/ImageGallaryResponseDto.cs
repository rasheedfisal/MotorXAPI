using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Requests
{
    public class ImageGallaryResponseDto
    {
        public Guid Id { get; set; }
        public Guid CarOfferId { get; set; }
        [Required]
        public string FilePath { get; set; }
    }
}
