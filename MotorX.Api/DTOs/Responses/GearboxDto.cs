using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Responses
{
    public class GearboxDto
    {
        public Guid Id { get; set; }
        [Required]
        public string GearboxName { get; set; }
        public string? GearboxNameAr { get; set; }
    }
}
