using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Requests
{
    public class UpdateCarOfferRequestDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public Guid BrandModelId { get; set; }
        public Guid? TrimId { get; set; }
        [Required]
        public Guid YearId { get; set; }
        [Required]
        public Guid ColorId { get; set; }
        public Guid CartTypeId { get; set; }
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

        public string? YTLink { get; set; }
        public string? UserId { get; set; }
    }
}
