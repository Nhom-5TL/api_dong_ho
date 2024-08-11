using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace api_dong_ho.Dtos
{
    public class giohang
    {
        public int MaSP { get; set; }
        public string HinhAnh { get; set; }
        public string TenSP { get; set; }
        public string TenKT { get; set; }
        public string TenMS { get; set; }
        [Range(1, 99999999, ErrorMessage = "Số lượng không được nhỏ hơn 1")]
        public int SoLuong { get; set; }
        public int? gia { get; set; }
        public int MaKH { get; set; }
        public decimal ThanhTien { get; set; }

    }
    public class MySetting
    {
        public static string GioHang_KEY = "MYCART";
        public static string CLAIM_MAKH = "MaKh";
    }
    public class GioHangRequest
    {
        public int maSP { get; set; }
        public int maKT { get; set; }
        public int maMS { get; set; }

        public int maKH { get; set; }
        public int SoLuong { get; set; }
    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
    public class DonHanguse
    {
        public int? Id { get; set; }
        public int? MaSP { get; set; }
        public string Hinha { get; set; }
        public string Tensp { get; set; }
        public string TenKT { get; set; }
        public string TenMS { get; set; }
        public int SoLuong { get; set; }
        public int? Gia { get; set; }
        public string TenKh { get; set; }
        public string diaChi { get; set; }
        public string sdt { get; set; }
        public int TinhTrang { get; set; }
        public decimal giamgia { get; set; }
        public string idkh { get; set; }
        public string TrangThaiThanhToan { get; set; }
        public string? GhiChu { get; set; }
        public DateTime? NgayNhan { get; set; }
        public DateTime? NgayGiao { get; set; }
        public DateTime? NgayHuy { get; set; }
    }
}
