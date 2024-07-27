using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api_dong_ho.Models
{
    public class MauSac
    {
        [Key]
        public int MaMauSac { get; set; }
        public string TenMauSac { get; set; }
        public int? MaSP { get; set; }
        [ForeignKey("MaSP")]
        public SanPham? SanPham { get; set; }
    }
}
