using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_dong_ho.Models
{
    public class HinhAnh
    {
        [Key]
        public int MaHinhAnh { get; set; }

        public string? TenHinhAnh { get; set; }

        public int MaSanPham { get; set; }
        [ForeignKey("MaSanPham")]
        public SanPham? SanPham { get; set; }
    }
}
