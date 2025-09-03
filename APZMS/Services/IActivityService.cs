using APZMS.DTOs;
using APZMS.Models;

namespace APZMS.Services
{
    public interface IActivityService
    {
        Task<ActivityGames> AddActivityAsync(AddActivityDto dto);
        Task<IEnumerable<ActivityResponseDto>> GetActivitiesAsync(string? ageGroup, string? safetyLevel);
    }
}
