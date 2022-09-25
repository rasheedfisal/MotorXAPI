namespace MotorX.Api.DTOs.Responses
{
    public class CustomerSummaryDto
    {
        public Guid Id { get; set; }
        public string ModelName { get; set; }
        public string? ModelNameAr { get; set; }
        public string BrandName { get; set; }
        public string? BrandNameAr { get; set; }
        public string FullCarName => $"{BrandName} {ModelName}";
        public string? FullCarNameAr => $"{BrandNameAr} {ModelNameAr}";
    }
}
