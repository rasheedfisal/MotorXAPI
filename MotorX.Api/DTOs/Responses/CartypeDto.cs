using System.ComponentModel.DataAnnotations;

namespace MotorX.Api.DTOs.Responses
{
    public class CartypeDto
    {
        public Guid Id { get; set; }
        [Required]
        public string TypeName { get; set; }
        public string? TypeNameAr { get; set; }
        public string? ImgPath { get; set; }
    }
}
