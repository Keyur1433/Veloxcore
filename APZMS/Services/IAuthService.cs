using APZMS.DTOs;
using APZMS.Utilities;

namespace APZMS.Services
{
    public interface IAuthService
    {
        Task<ServiceResult<LoginResponseDto>> LoginAsync(UserLoginDto dto);
    }
}
