namespace backend.DTO
{
    public class CartDto
    {
        public long UserId { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
        public decimal TotalPrice => Items.Sum(i => i.TotalPrice);
    }
}
