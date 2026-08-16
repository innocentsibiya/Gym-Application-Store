namespace backend.DTO
{
    public class PaymentDto
    {
        public string Method { get; set; } = "card";
        public string? CardNumber { get; set; }
        public string? CVV { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
    }
}
