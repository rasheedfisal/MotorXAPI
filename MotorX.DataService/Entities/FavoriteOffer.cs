using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.Entities
{
    public class FavoriteOffer
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();    
        public string UserId { get; set; }
        public Guid OfferId { get; set; }
        [ForeignKey(nameof(OfferId))]
        public CarOffer Offer { get; set; }
        public bool IsFavorite { get; set; } = false;
    }
}
