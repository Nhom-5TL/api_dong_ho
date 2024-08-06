using System.ComponentModel.DataAnnotations;

namespace api_dong_ho.Dtos
{
    public class DonHangRequest
    {  [Required]
        public string TenKh { get; set; }

        [Required]
        public string DiaChi { get; set; }

        [Required]
        public string SDT { get; set; }

        public string GhiChu { get; set; }
        public int MaKH { get; set; }
        public string TrangThaiThanhToan { get; set; }

        [Required]
        public List<ChiTietDonHangRequest> ChiTietDonHangs { get; set; }
    }
    public class ChiTietDonHangRequest
    {
        public int MaSP { get; set; } // Ensure this is an integer
        public string? TenKT { get; set; }
        public string? tenMS { get; set; }
        public int SoLuong { get; set; }
        public int? DonGia { get; set; }
        public string TenSP { get; set; }
    }
}
