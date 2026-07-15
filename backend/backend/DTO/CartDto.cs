namespace backend.DTO
{
    public class CartDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
    }
}
