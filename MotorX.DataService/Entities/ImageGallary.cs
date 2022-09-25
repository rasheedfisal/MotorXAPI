using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.Entities
{
    public class ImageGallary: BaseEntity
    {
        [Required]
        public string FilePath { get; set; }
        [Required]
        public int OrderNo { get; set; }
        public Guid CarOfferId { get; set; }
        [ForeignKey(nameof(CarOfferId))]
        public CarOffer CarOffer { get; set; }
    }
}
