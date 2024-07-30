using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace api_dong_ho.Models
{
    public class Application : IdentityUser
    {
        [Required]
        public string TenKh { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string SDT { get; set; }
        [Required]
        public string CCCD { get; set; }
        [Required]
        public string TenTaiKhoan { get; set; }
        [Required]
        public string MatKhau { get; set; }
        [Required]
        public string TrangThai { get; set; }
    }
}
