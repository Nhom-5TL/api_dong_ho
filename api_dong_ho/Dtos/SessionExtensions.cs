using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace api_dong_ho.Dtos
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        public static string ErrorMessage = "Chọn hình ảnh có đuôi jpg, png, jpeg";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName)?.ToLower();
                string[] allowedExtensions = { ".jpg", ".png", ".jpeg" };

                if (!allowedExtensions.Contains(extension))
                {
                    ErrorMessage = "Chọn hình ảnh có đuôi jpg, png, jpeg";
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
