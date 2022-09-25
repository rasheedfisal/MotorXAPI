using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.Entities
{
    public class Features: BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string FeatureName { get; set; }
        [MaxLength(400)]
        public string? FeatureNameAr { get; set; }

        public Guid FeaturetypeId { get; set; }
        [ForeignKey(nameof(FeaturetypeId))]
        public FeaturesType FeaturesType { get; set; }
    }
}
