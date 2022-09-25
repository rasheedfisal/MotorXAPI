using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Responses
{
    public class LocationDto
    {
        public Guid Id { get; set; }
        [Required]
        public string LocationName { get; set; }
        public string? LocationNameAr { get; set; }
    }
}
