using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Requests
{
    public class BrandRequestDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? NameAr { get; set; }
        public IFormFile? Logo { get; set; }
    }
}
