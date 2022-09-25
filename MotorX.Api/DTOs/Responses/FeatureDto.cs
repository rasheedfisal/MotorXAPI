namespace MotorX.Api.DTOs.Responses
{
    public class FeatureDto
    {
        public Guid Id { get; set; }
        public string FeatureName { get; set; }
        public string? FeatureNameAr { get; set; }
        public bool isSelected { get; set; } = false;
    }
}
