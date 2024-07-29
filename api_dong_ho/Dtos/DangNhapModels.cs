using System.ComponentModel.DataAnnotations;

namespace api_dong_ho.Dtos
{
    public class DangNhapModels
    {
        [Required]
        public string TenDN { get; set; } = null!;
        [Required]
        public string MauKhau { get; set; } = null!;
    }
}
