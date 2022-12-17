using MotorX.Auth.DTO.Responses;

namespace MotorX.Api.DTOs.Responses
{
    public class MGetOfferDetailsDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public BrandModelDto BrandModel { get; set; }
        public TrimDto? Trim { get; set; }
        public YearDto Year { get; set; }
        public GearboxDto? Gearbox { get; set; }
        public SpecsDto? Specs { get; set; }
        public LocationDto? Location { get; set; }
        public ColorsDto Colors { get; set; }
        public int Seats { get; set; }
        public string? Kilometer { get; set; }
        public decimal Price { get; set; }
        public CurrencyDto Currency { get; set; }
        public bool IsActive { get; set; }

        public CartypeDto Cartype { get; set; }

        public List<ImageGallaryDto> ImageGallaries { get; set; }
        public List<FeaturetypeAllDto> FeaturesTypes { get; set; }
        public string? YTLink { get; set; }
        public bool IsFavorite { get; set; }
        public OfferUserResponse? User { get; set; }
    }
}
