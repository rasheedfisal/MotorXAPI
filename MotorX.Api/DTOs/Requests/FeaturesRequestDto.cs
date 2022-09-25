using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Requests
{
    public class FeaturesRequestDto
    {
        public Guid Id { get; set; }
        [Required]
        public string FeatureName { get; set; }
        public string? FeatureNameAr { get; set; }

        public Guid FeaturetypeId { get; set; }
    }
}
