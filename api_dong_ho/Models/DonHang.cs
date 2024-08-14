using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_dong_ho.Models
{
    public class DonHang
    {
        [Key]
        public int MaDH { get; set; }
        public string ?TenKh { get; set; }
        public string ?GhiChu { get; set; }
        public int MaKh { get; set; }
        [ForeignKey("MaKh")]
        public virtual KhachHang? KhachHang { get; set; }
        public DateTime NgayTao { get; set; }
        public int TrangThai { get; set; } = 0;
        public int TrangThaiTT { get; set; } = 0;
        public string TrangThaiThanhToan { get; set; } = "COD";
        public string LyDoHuy { get; set; } = "";
        public DateTime? NgayHuy { get; set; }
        public DateTime? NgayNhan { get; set; }
        public decimal TongTien {  get; set; }
        public string ?DiaChi { get; set; }
        public string ?SDT { get; set; }
        public string? TinhThanh { get; set; }
        public string? QuanHuyen { get; set; }
        public string? XaPhuong { get; set; }

        public ICollection<ChiTietDonHang>? ChiTietDonHangs { get; set; }
    }
}
