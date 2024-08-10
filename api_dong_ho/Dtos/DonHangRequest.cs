using System.ComponentModel.DataAnnotations;

namespace api_dong_ho.Dtos
{
    public class DonHangRequest
    {  
        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        [MinLength(5, ErrorMessage = "Ít nhất 5 ký tự")]
        public string TenKh { get; set; }

        [Required(ErrorMessage = "Địa Chỉ không được để trống")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        [MinLength(5, ErrorMessage = "Ít nhất 5 ký tự")]
        public string DiaChi { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [MaxLength(11, ErrorMessage = "Tối đa 11 ký tự")]
        [MinLength(9, ErrorMessage = "Ít nhất 9 ký tự")]
        [RegularExpression(@"0[983]\d{8}", ErrorMessage = "Vui lòng đúng định dạng số điện thoại (0 [983])")]
        public string SDT { get; set; }
        [Required(ErrorMessage = "Ghi Chú không được để trống")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        [MinLength(5, ErrorMessage = "Ít nhất 5 ký tự")]
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
