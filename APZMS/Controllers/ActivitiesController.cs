using APZMS.DTOs;
using APZMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APZMS.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivitiesController(IActivityService activityService)
        { 
            _activityService = activityService;
        }

        [HttpPost("activities")]
        [Authorize(Roles = "customer, staff")]
        public async Task<IActionResult> AddActivity([FromForm] AddActivityDto dto)
        { 
            try
            {
                var activity = await _activityService.AddActivityAsync(dto);

                return CreatedAtAction(nameof(AddActivity), new
                {
                    activityId = activity.Id,
                    activityName = activity.Name,
                    message = "Activity created successfully.",
                    photoUrl = activity.PhotoUrl,
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    errors = new[]
                    {
                        new {field = "ageRange", message = ex.Message}
                    }
                });
            }
        }

        [HttpGet("activities")]
        [Authorize(Roles = "admin,staff,customer")]
        public async Task<IActionResult> GetActivities([FromQuery] string? ageGroup, [FromQuery] string? safetyLevel)
        {
            var activities = await _activityService.GetActivitiesAsync(ageGroup, safetyLevel);
            return Ok(new { activities });
        }
    }
}
