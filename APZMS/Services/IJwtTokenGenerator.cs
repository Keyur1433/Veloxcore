using APZMS.Models;

namespace APZMS.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
