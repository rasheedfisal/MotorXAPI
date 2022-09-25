namespace MotorX.Api.DTOs.Responses
{
    public class MGetAllOfferDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public BrandModelDto BrandModel { get; set; }
        public YearDto Year { get; set; }
        public int Seats { get; set; }
        public string? Kilometer { get; set; }
        public decimal Price { get; set; }
        public CurrencyDto Currency { get; set; }
        public CartypeDto Cartype { get; set; }
        public bool IsActive { get; set; }

        public string? YTLink { get; set; }
        public string? MainImg { get; set; }
        public bool IsFavorite { get; set; }
    }
}
