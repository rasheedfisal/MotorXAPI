using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.Entities
{
    public class SummaryMostViewed: BaseEntity
    {
        [Required]
        public Guid CarOfferId { get; set; }
        [ForeignKey(nameof(CarOfferId))]
        public CarOffer CarOffer { get; set; }
        [Required]
        public int NumberOfViews { get; set; } = 1;
    }
}
