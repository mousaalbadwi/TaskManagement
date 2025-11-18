using TaskManagement.Api.Entities;

namespace TaskManagement.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
