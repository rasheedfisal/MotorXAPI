namespace MotorX.Api.DTOs.Requests
{
    public class UpdateCarTypeRequestDto
    {
        public Guid Id { get; set; }
        public string TypeName { get; set; }
        public string? TypeNameAr { get; set; }
        public IFormFile? ImgPath { get; set; }
    }
}
