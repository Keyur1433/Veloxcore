using APZMS.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace APZMS.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User> AddAsync(User user);
        DbSet<User> GetUsers();
        IQueryable<User> GetUserAsQueryable();
        Task<int> SaveChangesAsync();
    }
}
