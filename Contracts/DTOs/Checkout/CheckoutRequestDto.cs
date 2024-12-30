namespace Contracts.DTOs.Checkout
{
    public class CheckoutRequestDto
    {
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
        public string? SpecialRequests { get; set; }

    }
}
