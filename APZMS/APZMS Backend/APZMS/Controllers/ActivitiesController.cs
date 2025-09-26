using APZMS.Application.DTOs;
using APZMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APZMS.Controllers
{
    [ApiController]
    [Route("api/v1/activities")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivitiesController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpPost]
        [Authorize(Roles = "customer, staff")]
        public async Task<IActionResult> AddActivity(AddActivityDto dto)
        {
            var activity = await _activityService.AddActivityAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = activity.Id }, new
            {
                activityId = activity.Id,
                activityName = activity.Name,
                message = "Activity created successfully.",
                photoUrl = activity.PhotoUrl,
            });
        }

        [HttpGet]
        [Authorize(Roles = "admin,staff,customer")]
        public async Task<IActionResult> GetActivities([FromQuery] string? ageGroup, [FromQuery] string? safetyLevel)
        {
            var activities = await _activityService.GetActivitiesAsync(ageGroup, safetyLevel);
            return Ok(new { activities });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var activity = await _activityService.GetByIdAsync(id);

            if (activity == null) return NotFound();

            return Ok(activity);
        }
    }
}
