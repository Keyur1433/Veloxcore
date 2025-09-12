using APZMS.Application.DTOs;
using APZMS.Application.Common;

namespace APZMS.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<LoginResponseDto>> LoginAsync(UserLoginDto dto);
    }
}
