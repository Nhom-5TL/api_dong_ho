using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_dong_ho.Models
{
    public class ChiTietDonHang
    {
        [Key]
        public int MaCTDH { get; set; }
        public int? MaDH { get; set; }
        [ForeignKey("MaDH")]
        public DonHang? DonHang { get; set; }
        public int? MaSP { get; set; }
        [ForeignKey("MaSP")]
        public SanPham? SanPham { get; set; }
        public int SoLuong { get; set; }
        public int? DonGia { get; set; }
        public string TenSP { get; set; }
        public int ?MaMauSac { get; set; }
        [ForeignKey("MaMauSac")]
        public MauSac? MauSac { get; set; }
        public int ?MaKichThuoc { get; set; }
        [ForeignKey("MaKichThuoc")]
        public KichThuoc? KichThuoc { get; set; }
    }
}
