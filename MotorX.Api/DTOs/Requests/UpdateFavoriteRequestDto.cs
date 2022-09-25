namespace MotorX.Api.DTOs.Requests
{
    public class UpdateFavoriteRequestDto
    {
        public Guid OfferId { get; set; }
        public string ClientId { get; set; }
        public bool IsFavorite { get; set; }
    }
}
