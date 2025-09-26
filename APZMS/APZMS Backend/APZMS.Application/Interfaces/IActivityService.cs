using APZMS.Application.DTOs;
using APZMS.Domain.Models;

namespace APZMS.Application.Interfaces
{
    public interface IActivityService
    {
        Task<ActivityGames> AddActivityAsync(AddActivityDto dto);
        Task<IEnumerable<ActivityResponseDto>> GetActivitiesAsync(string? ageGroup, string? safetyLevel);

        // cursor
        Task<ActivityResponseDto?> GetByIdAsync(int id);
    }
}
