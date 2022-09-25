using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Requests
{
    public class AddModelRequestDto
    {
        [Required]
        public string ModelName { get; set; }
        public string? ModelNameAr { get; set; }
        public Guid BrandId { get; set; }
    }
}
