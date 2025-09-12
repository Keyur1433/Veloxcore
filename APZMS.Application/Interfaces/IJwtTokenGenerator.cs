using APZMS.Domain.Models;

namespace APZMS.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
