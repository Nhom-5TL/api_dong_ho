using api_dong_ho.Dtos;
using api_dong_ho.Models;
using System.ComponentModel.DataAnnotations;

public class PutSanPham
{
    public int MaSP { get; set; }

    [Required(ErrorMessage = "Yêu cầu nhập tên sản phẩm")]
    [StringLength(80, ErrorMessage = "Tên sản phẩm không được vượt quá 80 kí tự")]
    public string? TenSP { get; set; }

    [Required(ErrorMessage = "Yêu cầu nhập mô tả sản phẩm")]
    [StringLength(5000, ErrorMessage = "Mô tả sản phẩm không được vượt quá 5000 kí tự")]
    public string? MoTa { get; set; }

    [Required(ErrorMessage = "Yêu cầu nhập giá sản phẩm")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Giá sản phẩm chỉ được nhập số")]
    public int Gia { get; set; }

    [Required(ErrorMessage = "Yêu cầu chọn loại sản phẩm")]
    [Range(1, int.MaxValue, ErrorMessage = "Yêu cầu chọn loại sản phẩm")]
    public int MaLoai { get; set; }

    [Required(ErrorMessage = "Yêu cầu chọn nhãn hiệu sản phẩm")]
    [Range(1, int.MaxValue, ErrorMessage = "Yêu cầu chọn thương hiệu sản phẩm")]
    public int MaNhanHieu { get; set; }

    public List<string> HinhAnhHienTai { get; set; } = new List<string>();
    public List<IFormFile> HinhAnhMoi { get; set; } = new List<IFormFile>();

}



