using backend.Models;

namespace backend.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
