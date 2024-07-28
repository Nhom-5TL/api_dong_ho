using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_dong_ho.Models
{
    public class DonHang
    {
        [Key]
        public int MaDH { get; set; }
        public string TenKh { get; set; }
        public string GhiChu { get; set; }
        public int MaKh { get; set; }
        [ForeignKey("MaKh")]
        public virtual KhachHang? KhachHang { get; set; }
        public DateTime NgayTao { get; set; }
        public string TrangThai { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }

        public ICollection<ChiTietDonHang>? ChiTietDonHangs { get; set; }
    }
}
