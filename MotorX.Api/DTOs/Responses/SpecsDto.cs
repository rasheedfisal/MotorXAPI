using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Responses
{
    public class SpecsDto
    {
        public Guid Id { get; set; }
        [Required]
        public string SpecsName { get; set; }
        public string? SpecsNameAr { get; set; }
    }
}
