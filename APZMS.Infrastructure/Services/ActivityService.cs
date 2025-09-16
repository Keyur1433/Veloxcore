using APZMS.Application.DTOs;
using APZMS.Application.Interfaces;
using APZMS.Domain.Exceptions;
using APZMS.Domain.Models;
using APZMS.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace APZMS.Infrastructure.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IActivityRepository _activityRepository;

        public ActivityService(IWebHostEnvironment env, IActivityRepository activityRepository)
        {
            _env = env;
            _activityRepository = activityRepository;
        }

        public async Task<ActivityGames> AddActivityAsync(AddActivityDto dto)
        {
            if(dto.MinAge > dto.MaxAge)
            {
                throw new InvalidAgeRangeException("Maximum age must be greater than minimum age.");
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

            await _activityRepository.AddAsync(activity);
            await _activityRepository.SaveChangesAsync();

            return activity;    
        }

        public async Task<IEnumerable<ActivityResponseDto>> GetActivitiesAsync(string? ageGroup, string? safetyLevel)
        {
            var query = _activityRepository.GetActivityAsQueryable();

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

        // cursor
        public async Task<ActivityResponseDto?> GetByIdAsync(int id)
        {
            var entity = await _activityRepository.GetByIdAsync(id);
            if (entity == null) return null;

            return new ActivityResponseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                Capacity = entity.Capacity,
                MinAge = entity.MinAge,
                MaxAge = entity.MaxAge,
                SafetyLevel = entity.SafetyLevel,
                PhotoUrl = entity.PhotoUrl
            };
        }
    }
}
