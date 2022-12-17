using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.Entities
{
    public class CarOffer: BaseEntity
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public Guid BrandModelId { get; set; }
        [ForeignKey(nameof(BrandModelId))]
        public BrandModel BrandModel { get; set; }
        public Guid? TrimId { get; set; }
        [ForeignKey(nameof(TrimId))]
        public Trim Trim { get; set; }
        [Required]
        public Guid YearId { get; set; }
        [ForeignKey(nameof(YearId))]
        public Year Year { get; set; }
        [Required]
        public Guid ColorId { get; set; }
        [ForeignKey(nameof(ColorId))]
        public Colors Colors { get; set; }

        public Guid CarTypeId { get; set; }
        [ForeignKey(nameof(CarTypeId))]
        public Cartype Cartype { get; set; }
        public Guid? GearboxId { get; set; }
        [ForeignKey(nameof(GearboxId))]
        public Gearbox Gearbox { get; set; }
        public Guid? SpecsId { get; set; }
        [ForeignKey(nameof(SpecsId))]
        public Specs Specs { get; set; }
        public Guid? LocationId { get; set; }
        [ForeignKey(nameof(LocationId))]
        public Location Location { get; set; }
        [Required]
        public int Seats { get; set; }
        [Required]
        public string? Kilometer { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public Guid CurrencyId { get; set; }
        [ForeignKey(nameof(CurrencyId))]
        public Currency Currency { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public ICollection<ImageGallary> ImageGallaries { get; set; }
        public ICollection<CarFeatures> CarFeatures { get; set; }
        
        [MaxLength(500)]
        public string? YTLink { get; set; }

        public ICollection<FavoriteOffer> Favorite { get; set; }

        public string? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser AppUser { get; set; }
    }
}
