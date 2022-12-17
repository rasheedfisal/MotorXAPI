namespace MotorX.Api.DTOs.Responses
{
    public class NotificationResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
      
        public string Description { get; set; }
        public string? ImgPath { get; set; }
    }
}
