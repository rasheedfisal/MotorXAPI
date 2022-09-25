using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.Entities
{
    public class CarFeatures: BaseEntity
    {
        [Required]
        public Guid CarOfferId { get; set; }
        [ForeignKey(nameof(CarOfferId))]
        public  CarOffer CarOffer { get; set; }
        [Required]
        public Guid FeatureId { get; set; }
        [ForeignKey(nameof(FeatureId))]
        public Features Features { get; set; }

    }
}
