using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Responses
{
    public class CurrencyDto
    {
        public Guid Id { get; set; }
        [Required]
        public string CurrencyName { get; set; }
        public string? CurrencyNameAr { get; set; }
    }
}
