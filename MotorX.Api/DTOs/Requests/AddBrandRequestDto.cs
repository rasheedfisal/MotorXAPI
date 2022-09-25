using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Requests
{
    public class AddBrandRequestDto
    {
        [Required]
        public string Name { get; set; }
        public string? NameAr { get; set; }
        public IFormFile Logo { get; set; }
    }
}
