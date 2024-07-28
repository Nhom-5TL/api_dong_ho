using System.ComponentModel.DataAnnotations;

namespace api_dong_ho.Models
{
    public class KhachHang
    {
        [Key]
        public int MaKH { get; set; }
        public string TenKh { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string CCCD { get; set; }
        public string TenTaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string TrangThai { get; set; }
        public DateTime NgayTao { get; set; }
        public ICollection<DonHang>? DonHangs { get; set; }
    }
}
