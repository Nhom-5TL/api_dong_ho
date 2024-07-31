using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using api_dong_ho.Models;

namespace api_dong_ho.Models
{
    public class SanPham
    {
        [Key]
        public int MaSP { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập tên sản phẩm")]
        [StringLength(80, ErrorMessage = "Tên sản phẩm không được vượt quá 80 kí tự")]
        public string TenSP { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập mô tả sản phẩm")]
        [StringLength(5000, ErrorMessage = "Mô tả sản phẩm không được vượt quá 5000 kí tự")]
        public string MoTa { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập giá sản phẩm")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Giá sản phẩm chỉ được nhập số")]
        public int Gia { get; set; }

        public int HTVC { get; set; } = 1;


        public int TrangThai { get; set; } = 1;

        [Required(ErrorMessage = "Yêu cầu chọn loại sản phẩm")]
        [Range(1, int.MaxValue, ErrorMessage = "Yêu cầu chọn loại sản phẩm")]
        public int MaLoai { get; set; }
        [ForeignKey("MaLoai")]

        public Loai? Loais { get; set; }

        [Required(ErrorMessage = "Yêu cầu chọn nhãn hiệu sản phẩm")]
        [Range(1, int.MaxValue, ErrorMessage = "Yêu cầu chọn thương hiệu sản phẩm")]
        public int MaNhanHieu { get; set; }
        [ForeignKey("MaNhanHieu")]
        public NhanHieu? NhanHieus { get; set; }

        public ICollection<HinhAnh>? HinhAnhs { get; set; }

        //[NotMapped]
        //public IFormFile? HinhAnhTaiLen { get; set; }
        public ICollection<MauSac>? MauSacs { get; set; }
        public ICollection<KichThuoc>? KichThuocs { get; set; }
        public ICollection<ChiTietDonHang>? ChiTietDonHangs { get; set; }
    }
}
