using TaskManagement.Api.DTOs.Auth;

namespace TaskManagement.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task SignupAsync(SignupRequestDto request);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}
