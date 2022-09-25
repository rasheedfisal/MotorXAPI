using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Requests
{
    public class BrandModelRequestDto
    {
        public Guid Id { get; set; }
        [Required]
        public string ModelName { get; set; }
        public string? ModelNameAr { get; set; }
        public Guid BrandId { get; set; }
    }
}
