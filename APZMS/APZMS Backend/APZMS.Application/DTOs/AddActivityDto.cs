using APZMS.Application.Validators;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace APZMS.Application.DTOs
{
    public class AddActivityDto
    {
        [Required(ErrorMessage = "Activity name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Capacity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive integer.")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Minimum age is required.")]
        [Range(2, 99, ErrorMessage = "Minimum age must be between 2 and 99.")]
        public int MinAge { get; set; }

        [Required(ErrorMessage = "Maximum age is required.")]
        public int MaxAge { get; set; }

        [Required(ErrorMessage = "Safety level is required.")]
        [RegularExpression("^(low|medium|high)$", ErrorMessage = "Safety level must be 'low', 'medium', or 'high'.")]
        public string? SafetyLevel { get; set; }

        [Required(ErrorMessage = "Photo is required.")]
        [AllowedImage(new[] { ".jpg", ".jpeg", ".png" }, 5 * 1024 * 1024)]
        public IFormFile? Photo { get; set; }
    }
}
