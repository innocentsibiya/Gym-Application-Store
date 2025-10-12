namespace backend.DTO
{
    public class AuthResponseDto
    {
        public UserDto User { get; set; } = default!;
        public string Token { get; set; } = string.Empty;
    }
}
