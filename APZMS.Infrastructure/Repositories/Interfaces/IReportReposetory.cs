using APZMS.Domain.Models;

namespace APZMS.Infrastructure.Repositories.Interfaces
{
    public interface IReportRepository
    {
        IQueryable<ActivityGames> GetActivityAsQueryable();
    }
}
