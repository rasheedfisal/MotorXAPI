using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Responses
{
    public class ColorsDto
    {
        public Guid Id { get; set; }
        [Required]
        public string ColorName { get; set; }
        public string? ColorNameAr { get; set; }
    }
}
