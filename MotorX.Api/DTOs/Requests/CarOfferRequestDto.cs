using MotorX.Api.DTOs.Responses;
using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Requests
{
    public class CarOfferRequestDto
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public Guid BrandModelId { get; set; }
        public Guid? TrimId { get; set; }
        [Required]
        public Guid YearId { get; set; }
        [Required]
        public Guid ColorId { get; set; }
        public Guid CarTypeId { get; set; }
        public Guid? GearboxId { get; set; }
        public Guid? SpecsId { get; set; }
        public Guid? LocationId { get; set; }
        [Required]
        public int Seats { get; set; }
        public string? Kilometer { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public Guid CurrencyId { get; set; }
        [Required]
        public bool IsActive { get; set; }

        //public List<CarGallaryRequestDto> ImageGallaries { get; set; }
        public List<IFormFile> ImageGallaries { get; set; }
        public List<CarFeaturesRequestDto> CarFeatures { get; set; }

        public string? YTLink { get; set; }
    }
}
