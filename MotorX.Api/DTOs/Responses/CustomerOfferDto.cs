namespace MotorX.Api.DTOs.Responses
{
    public class CustomerOfferDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
       
        public string Address { get; set; }
       
        public string PhoneNo { get; set; }
        public string? Email { get; set; }
        public MGetAllOfferDto Offer { get; set; }
    }
}
