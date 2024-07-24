using System.ComponentModel.DataAnnotations;

namespace api_dong_ho.Models
{
    public class Loai
    {
        [Key]
        public int MaLoai { get; set; }

        [Required(ErrorMessage = "không được để trống loại")]
        
        public string TenLoai { get; set; } = string.Empty;
    }
}
