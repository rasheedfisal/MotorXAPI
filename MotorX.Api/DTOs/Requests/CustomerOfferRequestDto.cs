using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Requests
{
    public class CustomerOfferRequestDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        public string? Email { get; set; }
        [Required]
        public Guid OfferId { get; set; }
    }
}
