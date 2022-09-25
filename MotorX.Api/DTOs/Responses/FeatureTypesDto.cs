using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Responses
{
    public class FeatureTypesDto
    {
        public Guid Id { get; set; }
        [Required]
        public string TypeName { get; set; }
        public string? TypeNameAr { get; set; }
    }
}
