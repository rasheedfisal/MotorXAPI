using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Responses
{
    public class BrandModelDto
    {
        public Guid Id { get; set; }
        [Required]
        public string ModelName { get; set; }
        public string? ModelNameAr { get; set; }
        public BrandDto Brand { get; set; }
    }
}
