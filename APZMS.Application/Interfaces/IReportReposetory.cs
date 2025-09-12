using APZMS.Domain.Models;

namespace APZMS.Application.Interfaces
{
    public interface IReportRepository
    {
        IQueryable<ActivityGames> GetActivityAsQueryable();
    }
}
