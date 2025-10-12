using backend.DTO;

namespace backend.Interfaces
{
    using backend.Models;
    using System.Threading.Tasks;

    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterDto dto);
        Task<AuthResult> LoginAsync(LoginDto dto);
    }
}