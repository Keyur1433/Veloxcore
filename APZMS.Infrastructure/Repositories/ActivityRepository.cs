using APZMS.Infrastructure.Database;
using APZMS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using APZMS.Infrastructure.Repositories.Interfaces;

namespace APZMS.Infrastructure.Repositories
{
    internal class ActivityRepository : IActivityRepository
    {
        private readonly AppDbContext _context;
        public ActivityRepository(AppDbContext context) => _context = context;

        public async Task<ActivityGames> AddAsync(ActivityGames activityGames)
        {
            await _context.Activities.AddAsync(activityGames);
            return activityGames;
        }

        public async Task<ActivityGames?> GetByIdAsync(int activityId)
        {
            return await _context.Activities.SingleOrDefaultAsync(a => a.Id == activityId);
        }

        public IQueryable<ActivityGames> GetActivityAsQueryable()
        {
            //AsQueryable explicitly casts the DbSet<> to IQueryable<Activity> so that:
            //You can dynamically build queries(e.g., using .Where(), .OrderBy(), .Skip(), .Take() etc.).

            return _context.Activities.AsQueryable();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
