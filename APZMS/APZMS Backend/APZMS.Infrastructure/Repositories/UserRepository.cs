using APZMS.Infrastructure.Database;
using APZMS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using APZMS.Infrastructure.Repositories.Interfaces;

namespace APZMS.Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) => _context = context;

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            return user;
        }

        public DbSet<User> GetUsers()
        {
            return _context.Users;
        }

        public IQueryable<User> GetUserAsQueryable()
        {
            return _context.Users.AsQueryable();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
