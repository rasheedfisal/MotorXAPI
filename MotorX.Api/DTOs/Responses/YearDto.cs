using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Responses
{
    public class YearDto
    {
        public Guid Id { get; set; }
        [Required]
        public int YearName { get; set; }
    }
}
