namespace MotorX.Api.DTOs.Requests
{
    public class ProfileUpdateRequest
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? CurrentPassword { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
