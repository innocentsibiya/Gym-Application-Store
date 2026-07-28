namespace backend.DTO
{
    public class AddressDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string AddressType { get; set; } = "Shipping";
        public bool IsDefault { get; set; } = false;
    }
}
