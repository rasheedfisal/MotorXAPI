using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.Entities
{
    public class BrandModel: BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string ModelName { get; set; }
        [MaxLength(400)]
        public string? ModelNameAr { get; set; }
        public Guid BrandId { get; set; }
        [ForeignKey(nameof(BrandId))]
        public Brand Brand { get; set; }
    }
}
