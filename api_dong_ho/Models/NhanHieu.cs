using System.ComponentModel.DataAnnotations;

namespace api_dong_ho.Models
{
    public class NhanHieu
    {
        [Key]
        public int MaNhanHieu { get; set; }

        [Required(ErrorMessage = "không được để trống nhãn hiệu")]

        public string TenNhanHieu { get; set; } = string.Empty;
    }
}
