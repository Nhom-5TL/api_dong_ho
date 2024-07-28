using System.ComponentModel.DataAnnotations;

namespace api_dong_ho.Dtos
{
    public class giohang
    {
        public int Id { get; set; }
        public string Hinha { get; set; }
        public string Tensp { get; set; }
        public string TenKt { get; set; }
        public string TenMS { get; set; }
        public double giamgia { get; set; }
        [Range(1, 99999999, ErrorMessage = "Số lượng không được nhỏ hơn 1")]
        public int Soluong { get; set; }
        public int? giaB { get; set; }
        public decimal ThanhTien { get; set; }
    }
    public class MySetting
    {
        public static string GioHang_KEY = "MYCART";
    }
    public class GioHangRequest
    {
        public int Id { get; set; }
        public int SoLuong { get; set; }
    }
}
