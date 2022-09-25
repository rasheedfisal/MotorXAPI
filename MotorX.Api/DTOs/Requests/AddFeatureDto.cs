namespace MotorX.Api.DTOs.Requests
{
    public class AddFeatureDto
    {
        public string FeatureName { get; set; }
        public string? FeatureNameAr { get; set; }

        public Guid FeaturetypeId { get; set; }
    }
}
