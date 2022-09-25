namespace MotorX.Api.DTOs.Requests
{
    public class CarTypeRequestDto
    {
        public string TypeName { get; set; }
        public string? TypeNameAr { get; set; }
        public IFormFile? ImgPath { get; set; }
    }
}
