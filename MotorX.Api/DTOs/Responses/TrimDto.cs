using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Responses
{
    public class TrimDto
    {
        public Guid Id { get; set; }
        [Required]
        public string TrimName { get; set; }
        public string? TrimNameAr { get; set; }
    }
}
