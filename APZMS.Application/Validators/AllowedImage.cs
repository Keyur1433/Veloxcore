using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace APZMS.Application.Validators
{
    public class AllowedImage : ValidationAttribute
    {
        private readonly string[] _extension;
        private readonly long _maxFileSize;

        public AllowedImage(string[] extension, long maxFileSize)
        {
            _extension = extension;
            _maxFileSize = maxFileSize;
        }
                    
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is IFormFile file)
            {
                var ext = Path.GetExtension(file.FileName).ToLower();

                if (!_extension.Contains(ext))
                    return new ValidationResult($"Photo must be {String.Join(", ", _extension)}");
                
                if(file.Length > _maxFileSize)
                    return new ValidationResult($"Photo must be ≤ {_maxFileSize / (1024 * 1024)}MB.");
            }

            return ValidationResult.Success;
        }
    }
}