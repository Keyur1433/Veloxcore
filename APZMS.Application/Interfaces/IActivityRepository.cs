using APZMS.Domain.Models;

namespace APZMS.Application.Interfaces
{
    public interface IActivityRepository
    {
        Task<ActivityGames?> GetByIdAsync(int id);
        Task<ActivityGames> AddAsync(ActivityGames activityGames);
        IQueryable<ActivityGames> GetActivityAsQueryable();
        Task<int> SaveChangesAsync();
    }
}
