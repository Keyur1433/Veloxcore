using APZMS.Infrastructure.Database;
using APZMS.Domain.Models;
using APZMS.Application.Interfaces;

namespace APZMS.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<ActivityGames> GetActivityAsQueryable()
        {
            return _context.Activities.AsQueryable();
        }
    }
}
