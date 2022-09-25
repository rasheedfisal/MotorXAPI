using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.Entities
{
    public class OfferCustomerInfo: BaseEntity
    {
        [Required]
        [MaxLength(500)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(500)]
        public string Address { get; set; }
        [Required]
        [MaxLength(100)]
        public string PhoneNo { get; set; }
        [MaxLength(300)]
        public string? Email { get; set; }
        public bool MarkAsRead { get; set; }
        public Guid OfferId { get; set; }
        [ForeignKey(nameof(OfferId))]
        public CarOffer Offer { get; set; }
    }
}
