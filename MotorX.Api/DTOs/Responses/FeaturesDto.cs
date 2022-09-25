using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Responses
{
    public class FeaturesDto
    {
        public Guid Id { get; set; }
        [Required]
        public string FeatureName { get; set; }
        public string? FeatureNameAr { get; set; }
        [Required]
        public FeatureTypesDto FeaturesType { get; set; }
    }
}
