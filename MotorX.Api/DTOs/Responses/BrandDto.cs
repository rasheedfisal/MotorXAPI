using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Responses
{
    public class BrandDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? NameAr { get; set; }
        public string? LogoPath { get; set; }
    }
}
