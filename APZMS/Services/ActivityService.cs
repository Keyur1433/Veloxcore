using APZMS.Data;
using APZMS.DTOs;
using APZMS.Models;
using Microsoft.EntityFrameworkCore;

namespace APZMS.Services
{
    public class ActivityService : IActivityService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ActivityService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<ActivityGames> AddActivityAsync(AddActivityDto dto)
        {
            if(dto.MinAge > dto.MaxAge)
            {
                throw new ArgumentException("Maximum age must be greater than minimum age.");
            }

            var webRoot = _env.WebRootPath;
            if(string.IsNullOrWhiteSpace(webRoot))
            {
                webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            if(string.IsNullOrWhiteSpace(webRoot))
            {
                throw new InvalidOperationException("Web root path could not be determined.");
            }

            var uploadsFolder = Path.Combine(webRoot, "Uploads/Activities");
            Directory.CreateDirectory(uploadsFolder);

            var ext = Path.GetExtension(dto.Photo.FileName).ToLower();
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Photo.CopyToAsync(stream);
            }

            var activity = new ActivityGames
            {
                Name = dto.Name,
                Description = dto.Description.Trim(),
                Price = dto.Price,
                Capacity = dto.Capacity,
                MinAge = dto.MinAge,
                MaxAge = dto.MaxAge,
                SafetyLevel = dto.SafetyLevel.ToLower(),
                PhotoUrl = $"/Uploads/Activities/{fileName}"
            };

            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();

            return activity;    
        }

        public async Task<IEnumerable<ActivityResponseDto>> GetActivitiesAsync(string? ageGroup, string? safetyLevel)
        {
            //AsQueryable explicitly casts the DbSet<> to IQueryable<Activity> so that:
            //You can dynamically build queries(e.g., using .Where(), .OrderBy(), .Skip(), .Take() etc.).
            var query = _context.Activities.AsQueryable();

            //Filter by safetyLevel
            if (!string.IsNullOrEmpty(safetyLevel))
            {
                query = query.Where(a => a.SafetyLevel.ToLower() == safetyLevel.ToLower());
            }

            // Filter by ageGroup
            if (!string.IsNullOrEmpty(ageGroup))
            {
                switch (ageGroup.ToLower())
                {
                    case "toddler":
                        query = query.Where(a => a.MinAge >= 2 && a.MaxAge <= 4);
                        break;
                    case "kid":
                        query = query.Where(a => a.MinAge >= 5 && a.MaxAge <= 12);
                        break;
                    case "teen":
                        query = query.Where(a => a.MinAge >= 13 && a.MaxAge <= 17);
                        break;
                    case "adult":
                        query = query.Where(a => a.MaxAge >= 18);
                        break;
                    default:
                        query = query.Where(a => false);
                        break;
                }
            }

            return await query.Select(a => new ActivityResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Price = a.Price,
                Capacity = a.Capacity,
                MinAge = a.MinAge,
                MaxAge = a.MaxAge,
                SafetyLevel = a.SafetyLevel,
                PhotoUrl = a.PhotoUrl
            }).ToListAsync();
        }
    }
}
